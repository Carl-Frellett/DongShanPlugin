using CommandSystem;
using DongShanAPI.Hint;
using Exiled.Events.EventArgs;
using RemoteAdmin;
using System;
using Player = Exiled.API.Features.Player;
using DongShanAPI.SCA;
using PlayerEvent = Exiled.Events.Handlers.Player;
using XZPlugin;
using MEC;
using Exiled.API.Features;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Start682Command : ICommand
    {
        public string Command { get; } = "start682";
        public string[] Aliases { get; } = new string[] { };
        public string Description { get; } = "[ServerCommandAdmin] 指定某一名玩家成为682";

        int SCP682Re = 0;
        public Player SCP682;
        string DieHint = "<align=left><color=yellow><size=25>[收容通知]</color><color=red>SCP-682现已被收容</color></size></align>";
        string StartHint = "<b>你是\n<size=100><color=red>SCP-682</color></size>\n你拥有三条命,不断的死亡会让你不断的加强\n同时拥有对子弹的45%抗性</b>";
        string Re1_StartHint = "<b>你是\n<size=100><color=red>SCP-682</color></size>\n这是你的第二条命,你的血量与伤害被加强,同时体积增大至0.2倍</b>";
        string Re2_StartHint = "<b>你是\n<size=100><color=red>SCP-682</color></size>\n这是你的第三条命,你的血量与抗性被加强,同时体积增大至0.5倍</b>";


        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender is PlayerCommandSender playerSender))
            {
                response = "<color=red>错误：此命令只能由玩家使用</color>";
                return false;
            }

            Player player = Player.Get(playerSender.Nickname);
            if (player == null)
            {
                response = "<color=red>错误：找不到命令使用者</color>";
                return false;
            }

            if (!AdminFileManager.IsAdmin(player.Nickname, player.IPAddress))
            {
                response = "<color=red>错误：你没有管理员权限</color>";
                return false;
            }

            if (!player.ReferenceHub.serverRoles.RemoteAdmin)
            {
                response = "<color=red>错误：未经过管理员验证!</color>";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "<color=yellow>指令格式: .start682 <玩家ID></color>";
                return false;
            }

            if (!int.TryParse(arguments.At(0), out int targetId))
            {
                response = "<color=red>错误：玩家ID必须为整数</color>";
                return false;
            }

            Player target = Player.Get(targetId);
            if (target == null)
            {
                response = $"<color=red>错误：未找到ID为{targetId}的玩家</color>";
                return false;
            }
            foreach (Player p in Player.List)
            {
                p.SendConsoleMessage($"{playerSender.Nickname}将{target.Nickname}变成SCP682!", "管理员操作日志");
            }

            SCP682 = target;
            Start682(target);

            response = $"<color=green>成功,{target.Nickname}已成为SCP682</color>";
            return true;
        }
        public void Start682(Player player)
        {
            player.RueiHint(300, StartHint, 10);

            player.SetRole(RoleType.Scp93953);
            player.DisplayNickname = $"{player.Id} | SCP682 | {player.Nickname}";

            PlayerEvent.Dying += OnDie;
            PlayerEvent.Hurting += OnHurt;
            PlayerEvent.Died += OnDiee;

            SCP682Re++;
            Log.Info($"当前的次数: {SCP682Re}");
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
            if (ev.Target == SCP682 && SCP682Re == 1 && SCP682Re == 2)
            {
                if (ev.DamageType.isWeapon)
                {
                    ev.Amount /= 45;
                }
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
