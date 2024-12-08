// -----------------------------------------------------------------------
// <copyright file="Hurting.cs" company="Exiled Team">
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
    /// Patches <see cref="PlayerStats.HurtPlayer(PlayerStats.HitInfo, GameObject, bool)"/>.
    /// Adds the <see cref="Player.Hurting"/> event.
    /// </summary>
    [HarmonyPatch(typeof(PlayerStats), nameof(PlayerStats.HurtPlayer))]
    internal static class Hurting
    {
        private static bool Prefix(PlayerStats __instance, ref PlayerStats.HitInfo info, GameObject go)
        {
            try
            {
                if (go == null)
                    return true;

                API.Features.Player attacker = API.Features.Player.Get(info.IsPlayer ? info.RHub.gameObject : __instance.gameObject);
                API.Features.Player target = API.Features.Player.Get(go);

                if (target == null || target.IsHost)
                    return true;

                if (info.GetDamageType() == DamageTypes.Recontainment && target.Role == RoleType.Scp079)
                {
                    Scp079.OnRecontained(new RecontainedEventArgs(target));
                    var eventArgs = new DiedEventArgs(null, target, info);
                    Player.OnDied(eventArgs);
                }

                if (attacker == null || attacker.IsHost)
                    return true;

                var ev = new HurtingEventArgs(attacker, target, info);

                if (ev.Target.IsHost)
                    return true;

                Player.OnHurting(ev);

                info = ev.HitInformations;

                if (!ev.IsAllowed)
                    return false;

                if (!ev.Target.IsGodModeEnabled && (ev.Amount == -1 || ev.Amount >= ev.Target.Health + ev.Target.AdrenalineHealth))
                {
                    var dyingEventArgs = new DyingEventArgs(ev.Attacker, ev.Target, ev.HitInformations);

                    Player.OnDying(dyingEventArgs);

                    if (!dyingEventArgs.IsAllowed)
                        return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Exiled.API.Features.Log.Error($"Exiled.Events.Patches.Events.Player.Hurting: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}
