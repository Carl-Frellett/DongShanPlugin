// -----------------------------------------------------------------------
// <copyright file="Events.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events
{
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Loader;
    using HarmonyLib;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Patch and unpatch events into the game.
    /// </summary>
    public sealed class Events : Plugin<Config>
    {
#pragma warning disable 0618
        private static readonly Lazy<Events> LazyInstance = new Lazy<Events>(() => new Events());
        private readonly InternalHandlers.Round round = new InternalHandlers.Round();

        /// <summary>
        /// The below variable is used to increment the name of the harmony instance, otherwise harmony will not work upon a plugin reload.
        /// </summary>
        private int patchesCounter;

        private Events()
        {
        }

        /// <summary>
        /// The custom <see cref="EventHandler"/> delegate.
        /// </summary>
        /// <typeparam name="TEventArgs">The <see cref="EventHandler{TEventArgs}"/> type.</typeparam>
        /// <param name="ev">The <see cref="EventHandler{TEventArgs}"/> instance.</param>
        public delegate void CustomEventHandler<TEventArgs>(TEventArgs ev)
            where TEventArgs : System.EventArgs;

        /// <summary>
        /// The custom <see cref="EventHandler"/> delegate, with empty parameters.
        /// </summary>
        public delegate void CustomEventHandler();

        /// <summary>
        /// Gets the plugin instance.
        /// </summary>
        public static Events Instance => LazyInstance.Value;

        /// <summary>
        /// Gets a list of types and methods for which EXILED patches should not be run.
        /// </summary>
        [Obsolete("Use DisabledPatchesHashSet instead.")]
        public static List<MethodBase> DisabledPatches { get; } = new List<MethodBase>();

        /// <summary>
        /// Gets a set of types and methods for which EXILED patches should not be run.
        /// </summary>
        public static HashSet<MethodBase> DisabledPatchesHashSet { get; } = new HashSet<MethodBase>();

        /// <inheritdoc/>
        public override PluginPriority Priority { get; } = PluginPriority.First;

        /// <summary>
        /// Gets the <see cref="HarmonyLib.Harmony"/> instance.
        /// </summary>
        public Harmony Harmony { get; private set; }

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            base.OnEnabled();

            Patch();

            Handlers.Server.WaitingForPlayers += round.OnWaitingForPlayers;
            Handlers.Server.RestartingRound += round.OnRestartingRound;
            Handlers.Server.RoundStarted += round.OnRoundStarted;

            Handlers.Player.ChangingRole += round.OnChangingRole;

            Handlers.Player.Joined += round.OnJoined;

            ServerConsole.ReloadServerName();
            Scp096.MaxShield = Config.Scp096MaxShieldAmount;
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            base.OnDisabled();

            Unpatch();

            DisabledPatchesHashSet.Clear();
            DisabledPatches.Clear();

            Handlers.Server.WaitingForPlayers -= round.OnWaitingForPlayers;
            Handlers.Server.RestartingRound -= round.OnRestartingRound;
            Handlers.Server.RoundStarted -= round.OnRoundStarted;

            Handlers.Player.ChangingRole -= round.OnChangingRole;

            Handlers.Player.Joined -= round.OnJoined;
        }

        /// <summary>
        /// Patches all events.
        /// </summary>
        public void Patch()
        {
            try
            {
                Harmony = new Harmony($"exiled.events.{++patchesCounter}");
#if DEBUG
                var lastDebugStatus = Harmony.DEBUG;
                Harmony.DEBUG = true;
#endif
                Harmony.PatchAll();
#if DEBUG
                Harmony.DEBUG = lastDebugStatus;
#endif
                Log.Debug("Events patched successfully!", Loader.ShouldDebugBeShown);
            }
            catch (Exception exception)
            {
                Log.Error($"Patching failed! {exception}");
            }
        }

        /// <summary>
        /// Checks the <see cref="DisabledPatches"/> list and un-patches any methods that have been defined there. Once un-patching has been done, they can be patched by plugins, but will not be re-patchable by Exiled until a server reboot.
        /// </summary>
        public void ReloadDisabledPatches()
        {
            foreach (MethodBase method in DisabledPatches)
            {
                DisabledPatchesHashSet.Add(method);
            }

            foreach (MethodBase method in DisabledPatchesHashSet)
            {
                Harmony.Unpatch(method, HarmonyPatchType.All, Harmony.Id);
                Log.Info($"Unpatched {method.Name}");
            }
        }

        /// <summary>
        /// Unpatches all events.
        /// </summary>
        public void Unpatch()
        {
            Log.Debug("Unpatching events...", Loader.ShouldDebugBeShown);

            Harmony.UnpatchAll();

            Log.Debug("All events have been unpatched complete. Goodbye!", Loader.ShouldDebugBeShown);
        }
    }
}
