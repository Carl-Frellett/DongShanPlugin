using Exiled.API.Features;
using MEC;
using System.Collections.Generic;
using Exiled.Events.EventArgs;
using System.Linq;
using DongShanAPI.Hint;

namespace XZPlugin
{
    public class TeamNum
    {
        private Dictionary<Player, bool> Players = new Dictionary<Player, bool>();

        public void OnJoin(JoinedEventArgs ev)
        {
            Start(ev.Player);
            Players[ev.Player] = true;
        }
        public void OnRoundStarted()
        {
            Players.Clear();

            foreach (var player in Player.List)
            {
                if (player.Role == RoleType.Spectator || player.Role == RoleType.None)
                {
                    Players[player] = false;
                }
            }
        }

        private Dictionary<Player, CoroutineHandle> Coroutines = new Dictionary<Player, CoroutineHandle>();

        private void Start(Player player)
        {
            if (Coroutines.TryGetValue(player, out var StopCoroutine))
            {
                Timing.KillCoroutines(StopCoroutine);
                Coroutines.Remove(player);
            }

            if (!Players.ContainsKey(player) || !Players[player])
            {
                var coroutine = Timing.RunCoroutine(Hint(player));
                Coroutines[player] = coroutine;
                Players[player] = true;
            }
        }

        private IEnumerator<float> Hint(Player player)
        {
            while (true)
            {
                if (!player.IsDead && player.Role != RoleType.Tutorial && player.Team != Team.SCP)
                {
                    int ClassD = Player.List.Count(p => p.Team == Team.CDP);
                    int CI = Player.List.Count(p => p.Role == RoleType.ChaosInsurgency);
                    int 科学家 = Player.List.Count(p => p.Role == RoleType.Scientist);
                    int NTF = Player.List.Count(p => p.Team == Team.MTF && p.Role != RoleType.FacilityGuard);
                    int gruod = Player.List.Count(p => p.Role == RoleType.FacilityGuard);

                    string message = "";

                    switch (player.Team)
                    {
                        case Team.CDP:
                        case Team.CHI:
                            message =
                                $"<line-height=30><align=right><size=25><color=green><b>混沌分裂者</b></color>: {CI}\n<color=orange>D<b>级人员</b></color>: {ClassD}</size></align>";
                            break;

                        case Team.RSC:
                        case Team.MTF:
                        case (Team)RoleType.FacilityGuard:
                            message =
                                $"<line-height=30><align=right><size=25><color=blue><b>九尾狐</b>: </color>{NTF}\n<color=yellow><b>科学家</b>: </color>{科学家}\n<color=#808080><b>设施警卫</b>: </color>{gruod}</size></align>";
                            break;
                    }
                    player.RueiHint(400, message,1);
                }

                yield return Timing.WaitForSeconds(1f);
            }
        }
    }
}