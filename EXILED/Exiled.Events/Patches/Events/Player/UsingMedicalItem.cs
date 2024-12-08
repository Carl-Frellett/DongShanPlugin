// -----------------------------------------------------------------------
// <copyright file="UsingMedicalItem.cs" company="Exiled Team">
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
    using MEC;
    using System;

    /// <summary>
    /// Patches <see cref="ConsumableAndWearableItems.CallCmdUseMedicalItem"/>.
    /// Adds the <see cref="Player.UsingMedicalItem"/> event.
    /// </summary>
    [HarmonyPatch(typeof(ConsumableAndWearableItems), nameof(ConsumableAndWearableItems.CallCmdUseMedicalItem))]
    internal static class UsingMedicalItem
    {
        private static bool Prefix(ConsumableAndWearableItems __instance)
        {
            try
            {
                if (!__instance._interactRateLimit.CanExecute(true))
                    return false;

                __instance._cancel = false;
                if (__instance.cooldown > 0f)
                    return false;

                for (int i = 0; i < __instance.usableItems.Length; ++i)
                {
                    if (__instance.usableItems[i].inventoryID == __instance._hub.inventory.curItem &&
                        __instance.usableCooldowns[i] <= 0.0)
                    {
                        var ev = new UsingMedicalItemEventArgs(API.Features.Player.Get(__instance.gameObject), __instance._hub.inventory.curItem, __instance.usableItems[i].animationDuration);
                        Player.OnUsingMedicalItem(ev);

                        __instance.cooldown = ev.Cooldown;

                        if (ev.IsAllowed)
                            Timing.RunCoroutine(__instance.UseMedicalItem(i), Segment.FixedUpdate);
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                Exiled.API.Features.Log.Error($"Exiled.Events.Patches.Events.Player.UsingMedicalItem: {e}\n{e.StackTrace}");

                return true;
            }
        }
    }
}
