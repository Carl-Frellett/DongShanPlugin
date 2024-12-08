using DongShanAPI.Hint;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XZPlugin
{
    class WaitingPlayer
    {
        System.Random rng = new System.Random();

        public List<CoroutineHandle> coroutines = new List<CoroutineHandle>();

        public void OnWaitingForPlayers()
        {
            GameObject.Find("StartRound").transform.localScale = Vector3.zero;

            foreach (CoroutineHandle coroutine in coroutines)
            {
                Timing.KillCoroutines(coroutine);
            }
            coroutines.Clear();

            if (XZPlugin.Plugin.Instance.Config.DisplayWaitMessage)
                coroutines.Add(Timing.RunCoroutine(LobbyTimer()));
        }

        public void OnPlayerJoin(JoinedEventArgs ev)
        {
            if (!Round.IsStarted)
            {
                Timing.CallDelayed(0.1f, () =>
                {
                    ev.Player.Role = XZPlugin.Plugin.Instance.Config.RolesToChoose[rng.Next(XZPlugin.Plugin.Instance.Config.RolesToChoose.Count)];
                });

                Timing.CallDelayed(0.5f, () =>
                {
                    ev.Player.Position = NewRoom(RoomType.Surface);

                    if (!XZPlugin.Plugin.Instance.Config.AlowDamage)
                    {
                        ev.Player.IsGodModeEnabled = true;
                    }

                    if (XZPlugin.Plugin.Instance.Config.TurnedPlayers)
                    {
                        Scp096.TurnedPlayers.Add(ev.Player);
                        Scp173.TurnedPlayers.Add(ev.Player);
                    }

                    if (XZPlugin.Plugin.Instance.Config.ColaMultiplier != 0)
                    {
                        ev.Player.ReferenceHub.playerEffectsController.EnableEffect<CustomPlayerEffects.Scp207>(999f, false);
                        ev.Player.ReferenceHub.playerEffectsController.ChangeEffectIntensity<CustomPlayerEffects.Scp207>(XZPlugin.Plugin.Instance.Config.ColaMultiplier);
                    }
                });
            }
        }
        public void OnRoundStarted()
        {
            Timing.CallDelayed(0.25f, () =>
            {
                if (!XZPlugin.Plugin.Instance.Config.AlowDamage)
                {
                    foreach (Player ply in Player.List)
                    {
                        ply.IsGodModeEnabled = false;
                    }
                }

                if (XZPlugin.Plugin.Instance.Config.TurnedPlayers)
                {
                    Scp096.TurnedPlayers.Clear();
                    Scp173.TurnedPlayers.Clear();
                }

                foreach (CoroutineHandle coroutine in coroutines)
                {
                    Timing.KillCoroutines(coroutine);
                }
                coroutines.Clear();
            });
        }
        private Vector3 NewRoom(RoomType roomType)
        {
            Room room = Map.Rooms.FirstOrDefault(r => r.Type == roomType);

            if (room != null)
            {
                return room.Position + new Vector3(0,2,0);
            }

            return Vector3.zero;
        }

        private IEnumerator<float> LobbyTimer()
        {
            while (!Round.IsStarted)
            {
                StringBuilder message = new StringBuilder();

                message.Append(XZPlugin.Plugin.Instance.Translation.TopMessage);

                short NetworkTimer = GameCore.RoundStart.singleton.NetworkTimer;

                switch (NetworkTimer)
                {
                    case -2:
                        message.Replace("%seconds", XZPlugin.Plugin.Instance.Translation.ServerIsPaused);
                        break;
                    case -1:
                        message.Replace("%seconds", XZPlugin.Plugin.Instance.Translation.RoundIsBeingStarted);
                        break;
                    case 1:
                        message.Replace("%seconds", $"{NetworkTimer + 1} {XZPlugin.Plugin.Instance.Translation.OneSecondRemain}");
                        break;
                    default:
                        message.Replace("%seconds", $"{NetworkTimer + 1} {XZPlugin.Plugin.Instance.Translation.XSecondsRemains}");
                        break;
                }

                int NumOfPlayers = Player.List.Count();

                message.Append($"\n{XZPlugin.Plugin.Instance.Translation.BottomMessage}");

                if (NumOfPlayers == 1)
                    message.Replace("%players", $"{NumOfPlayers} {XZPlugin.Plugin.Instance.Translation.OnePlayerConnected}");
                else
                    message.Replace("%players", $"{NumOfPlayers} {XZPlugin.Plugin.Instance.Translation.XPlayersConnected}");

                string finalMessage = message.ToString();

                foreach (Player ply in Player.List)
                {
                    ply.RueiHint(300, finalMessage, 1);
                }

                yield return Timing.WaitForSeconds(1);
            }
        }
    }
}
