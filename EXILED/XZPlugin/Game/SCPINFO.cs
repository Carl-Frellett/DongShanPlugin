using DongShanAPI.Hint;
using Exiled.Events.EventArgs;
using MEC;
using System.Collections.Generic;
using System.Linq;
using Player = Exiled.API.Features.Player;

namespace XZPlugin
{
    class SCPINFO
    {
        private Dictionary<Player, CoroutineHandle> Coroutine = new Dictionary<Player, CoroutineHandle>();
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player != null)
            {
                RestartSCPInfo(ev.Player);
            }
        }

        private void RestartSCPInfo(Player player)
        {
            if (player == null)
            {
                return;
            }

            StopCoroutine(player);

            if (player != null)
            {
                CoroutineHandle value = Timing.RunCoroutine(StartSCPInfo(player));
                Coroutine[player] = value;
            }
        }

        private void StopCoroutine(Player player)
        {
            if (player != null && Coroutine.ContainsKey(player))
            {
                Timing.KillCoroutines(Coroutine[player]);
                Coroutine.Remove(player);
            }
        }

        private IEnumerator<float> StartSCPInfo(Player player)
        {
            while (player != null && player.ReferenceHub != null && player.IsScp == true)
            {
                string info = "<line-height=25><size=20><align=right>";
                int 小僵尸 = 0;

                foreach (var scp in Player.List.Where(p => p.Team == Team.SCP))
                {
                    switch (scp.Role)
                    {
                        case RoleType.Scp079:
                            if (scp.IsAlive)
                            {
                                var scp079Script = scp.ReferenceHub.scp079PlayerScript;
                                info += $"[{scp.Nickname}] <color=red>SCP-079</color>: AP: {scp079Script.Mana} XP: {scp079Script.Exp} Lv: {scp079Script.Lvl + 1}\n";
                            }
                            else
                            {
                                info += $"[{scp.Nickname}] <color=red>SCP-079</color>: <color=red>已死亡</color>";
                            }
                            break;
                        case RoleType.Scp0492:
                            if (scp.IsAlive) 小僵尸++;
                            break;
                        default:
                            string HP = scp.IsAlive ? $"{scp.Health}/{scp.MaxHealth}" : "<color=red>已死亡</color>";
                            info += $"[{scp.Nickname}] <color=red>{scp.Role}</color>: {HP}\n";
                            break;
                    }
                }

                if (小僵尸 > 0)
                {
                    info += $"<b>剩余小僵尸</b>: {小僵尸}";
                }

                info += "</size></align>";

                player.RueiHint(420, info, 1);

                yield return Timing.WaitForSeconds(1);
            }
        }
    }
}
