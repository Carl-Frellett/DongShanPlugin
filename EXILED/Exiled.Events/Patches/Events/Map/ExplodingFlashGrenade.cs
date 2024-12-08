// -----------------------------------------------------------------------
// <copyright file="ExplodingFlashGrenade.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Patches.Events.Map
{
#pragma warning disable SA1313
    using CustomPlayerEffects;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using Grenades;
    using HarmonyLib;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Patches <see cref="FlashGrenade.ServersideExplosion()"/>.
    /// Adds the <see cref="Handlers.Map.OnExplodingGrenade"/> event.
    /// </summary>
    [HarmonyPatch(typeof(FlashGrenade), nameof(FlashGrenade.ServersideExplosion))]
    internal static class ExplodingFlashGrenade
    {
        private static bool Prefix(FlashGrenade __instance)
        {
            try
            {
                Dictionary<Player, float> players = new Dictionary<Player, float>();

                foreach (GameObject gameObject in PlayerManager.players)
                {
                    Vector3 position = __instance.transform.position;
                    ReferenceHub hub = ReferenceHub.GetHub(gameObject);
                    Flashed effect = hub.playerEffectsController.GetEffect<Flashed>();
                    Deafened effect2 = hub.playerEffectsController.GetEffect<Deafened>();
                    if (effect == null || __instance.thrower == null ||
                        (!__instance.Network_friendlyFlash && !effect.Flashable(
                            ReferenceHub.GetHub(__instance.thrower.gameObject), position, __instance._ignoredLayers)))
                        continue;

                    float num = __instance.powerOverDistance.Evaluate(
                                    Vector3.Distance(gameObject.transform.position, position) / ((position.y > 900f)
                                        ? __instance.distanceMultiplierSurface
                                        : __instance.distanceMultiplierFacility)) *
                                __instance.powerOverDot.Evaluate(Vector3.Dot(hub.PlayerCameraReference.forward, (hub.PlayerCameraReference.position - position).normalized));
                    byte b = (byte)Mathf.Clamp(Mathf.RoundToInt(num * 10f * __instance.maximumDuration), 1, 255);
                    if (b >= effect.Intensity && num > 0f)
                    {
                        players.Add(Player.Get(gameObject), num);
                    }
                }

                ExplodingGrenadeEventArgs ev = new ExplodingGrenadeEventArgs(Player.Get(__instance.throwerGameObject), players, false, __instance.gameObject);

                Handlers.Map.OnExplodingGrenade(ev);

                return ev.IsAllowed;
            }
            catch (Exception exception)
            {
                Log.Error($"Exiled.Events.Patches.Events.Map.ExplodingFlashGrenade: {exception}\n{exception.StackTrace}");

                return true;
            }
        }
    }
}
