using DongShanAPI.Hint;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using PlayerEvent = Exiled.Events.Handlers.Player;
using MEC;
using System;
using System.Linq;

namespace XZPlugin
{
    public class S682
    {
        int SCP682Re = 0;
        public Player SCP682;
        string DieHint = "<align=left><color=yellow><size=25>[收容通知]</color><color=red>SCP-682现已被收容</color></size></align>";
        string StartHint = "<b>你是\n<size=100><color=red>SCP-682</color></size>\n你拥有三条命,不断的死亡会让你不断的加强\n同时拥有对子弹的45%抗性</b>";
        string Re1_StartHint = "<b>你是\n<size=100><color=red>SCP-682</color></size>\n这是你的第二条命,你的血量与伤害被加强,同时体积增大至0.2倍</b>";
        string Re2_StartHint = "<b>你是\n<size=100><color=red>SCP-682</color></size>\n这是你的第三条命,你的血量与伤害被加强,同时体积增大至0.5倍</b>";
        public void OnRoundStart()
        {
            Timing.CallDelayed(2f, () =>
            {
                Start682(SCP682);
            });
        }
        public void Start682(Player player)
        {
            var ply = Player.List.Where(p => p.Role == RoleType.Scp93989 || p.Role == RoleType.Scp93953).ToList();
            if (ply.Count > 9)
            {
                player = ply[new Random().Next(ply.Count)];

                player.RueiHint(300, StartHint, 10);

                player.DisplayNickname = $"{player.Id} | SCP682 | {player.Nickname}";

                PlayerEvent.Dying += OnDie;
                PlayerEvent.Hurting += OnHurt;
                PlayerEvent.Died += OnDiee;
            }
        }

        public void OnDie(DyingEventArgs ev)
        {
            if (SCP682Re == 1)
            {
                if (ev.Target == SCP682)
                {
                    SCP682Re++;
                    Log.Info($"当前的次数: {SCP682Re}");

                    Timing.CallDelayed(0.5f, () =>
                    {
                        ev.Target.SetRole(RoleType.Scp93953, true);
                        ev.Target.RueiHint(300, Re1_StartHint, 10);
                        ev.Target.MaxHealth = 4500;
                        ev.Target.Health = 4500;
                        Timing.CallDelayed(0.2f, () =>
                        {
                            ev.Target.MaxHealth = 4500;
                            ev.Target.Health = 4500;
                        });
                        ev.Target.Scale = new UnityEngine.Vector3(1.02f, 1.02f, 1.02f);
                    });
                }
            }
            else if (SCP682Re == 2)
            {
                if (ev.Target == SCP682)
                {
                    SCP682Re++;
                    Log.Info($"当前的次数: {SCP682Re}");

                    Timing.CallDelayed(0.5f, () =>
                    {
                        ev.Target.SetRole(RoleType.Scp93953, true);
                        ev.Target.RueiHint(300, Re2_StartHint, 10);
                        ev.Target.MaxHealth = 7682;
                        ev.Target.Health = 7682;
                        Timing.CallDelayed(0.2f, () =>
                        {
                            ev.Target.MaxHealth = 7682;
                            ev.Target.Health = 7682;
                        });
                        ev.Target.Scale = new UnityEngine.Vector3(1.1f, 1.1f, 1.1f);
                    });
                }
            }
            else if (SCP682Re == 3)
            {
                SCP682Re++;
                Log.Info($"当前的次数: {SCP682Re}");
            }
        }
        public void OnDiee(DiedEventArgs ev)
        {
            if (ev.Target == SCP682 && SCP682Re == 4)
            {
                Log.Info($"当前的次数: {SCP682Re}");
                ev.Target.DisplayNickname = $"{ev.Target.Id} | {ev.Target.Nickname}";
                SCP682 = null;
                ev.Target.RueiHint(300, DieHint, 10);

                PlayerEvent.Dying -= OnDie;
                PlayerEvent.Hurting -= OnHurt;
                PlayerEvent.Died -= OnDiee;
                SCP682Re = 0;
            }
        }
        public void OnHurt(HurtingEventArgs ev)
        {
            if (ev.Target == SCP682 && ev.DamageType.isWeapon && SCP682Re == 1)
            {
                ev.Amount /= 45;
            }
            if (ev.Target == SCP682 && ev.DamageType.isWeapon && SCP682Re == 2)
            {
                ev.Amount /= 45;
            }

            if (ev.Target == SCP682 && ev.DamageType.isWeapon && SCP682Re == 3)
            {
                ev.Amount /= 80;
            }

            if (ev.Attacker == SCP682 && SCP682Re == 2)
            {
                ev.Amount = 90;
            }
            else
            {
                ev.Amount = ev.Amount;
            }
        }
    }
}
