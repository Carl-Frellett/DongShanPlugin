// -----------------------------------------------------------------------
// <copyright file="Joined.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Patches.Events.Player
{
    using Exiled.Events.EventArgs;
    using Exiled.Events.Handlers;
    using Exiled.Loader.Features;
    using HarmonyLib;
    using MEC;
    using System;
    using UnityEngine;

    /// <summary>
    /// Patches <see cref="PlayerManager.AddPlayer(GameObject)"/>.
    /// Adds the <see cref="Player.Joined"/> event.
    /// </summary>
    [HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.NetworkIsVerified), MethodType.Setter)]
    internal static class Joined
    {
        private static void Prefix(CharacterClassManager __instance, bool value)
        {
            try
            {
                // UserId will always be empty/null if it's not in online mode
                if (!value || (string.IsNullOrEmpty(__instance.UserId) && CharacterClassManager.OnlineMode))
                    return;

                if (!API.Features.Player.Dictionary.TryGetValue(__instance.gameObject, out API.Features.Player player))
                {
                    player = new API.Features.Player(ReferenceHub.GetHub(__instance.gameObject));

                    API.Features.Player.Dictionary.Add(__instance.gameObject, player);
                }
                Timing.CallDelayed(2.5f, () =>
                {
                    API.Features.Log.SendRaw($"玩家 {player?.Nickname} ({player?.IPAddress}) ({player?.Id}) 加入了服务器", ConsoleColor.Green);
                    player.DisplayNickname = $"{player.Id} | {player.Nickname}";
                });
                if (PlayerManager.players.Count >= CustomNetworkManager.slots)
                    MultiAdminFeatures.CallEvent(MultiAdminFeatures.EventType.SERVER_FULL);

                Timing.CallDelayed(0.25f, () =>
                {
                    if (player?.IsMuted == true)
                        player.ReferenceHub.characterClassManager.SetDirtyBit(2UL);
                });

                var ev = new JoinedEventArgs(API.Features.Player.Get(__instance.gameObject));

                Player.OnJoined(ev);
            }
            catch (Exception exception)
            {
                API.Features.Log.Error($"{typeof(Joined).FullName}:\n{exception}");
            }
        }
    }
}
