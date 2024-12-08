// -----------------------------------------------------------------------
// <copyright file="SyncingData.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Patches.Events.Player
{
#pragma warning disable SA1313
    using Exiled.Events.EventArgs;
    using Exiled.Events.Handlers;
    using HarmonyLib;
    using System;
    using UnityEngine;

    /// <summary>
    /// Patches <see cref="AnimationController.CallCmdSyncData(byte, Vector2)"/>.
    /// Adds the <see cref="Player.SyncingData"/> event.
    /// </summary>
    [HarmonyPatch(typeof(AnimationController), nameof(AnimationController.CallCmdSyncData))]
    internal static class SyncingData
    {
        private static bool Prefix(AnimationController __instance, byte state, Vector2 v2)
        {
            try
            {
                if (!__instance._mSyncRateLimit.CanExecute(false))
                    return false;

                var ev = new SyncingDataEventArgs(API.Features.Player.Get(__instance.gameObject), v2, state);

                Player.OnSyncingData(ev);

                return ev.IsAllowed;
            }
            catch (Exception e)
            {
                Exiled.API.Features.Log.Error($"Exiled.Events.Patches.Events.Player.SyncingData: {e}\n{e.StackTrace}");

                return true;
            }
        }
    }
}
