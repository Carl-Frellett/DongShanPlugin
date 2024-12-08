using CommandSystem;
using DongShanAPI.Hint;
using Exiled.Events.EventArgs;
using RemoteAdmin;
using System;
using Player = Exiled.API.Features.Player;
using DongShanAPI.SCA;
using PlayerEvent = Exiled.Events.Handlers.Player;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Start181Command : ICommand
    {
        public string Command { get; } = "start181";
        public string[] Aliases { get; } = new string[] { };
        public string Description { get; } = "[ServerCommandAdmin] 指定某一名玩家成为181";

        public Player SCP181;
        string DieHint = "<align=left><color=yellow><size=25>[收容通知]<color=red> SCP-181现已被收容</color></size></align>";
        string StartHint = "<b>你是\n<size=150><color=red>SCP-181</color></size>\n有概率无卡开门, 有小概率免除伤害</b>";
        public readonly System.Random random = new System.Random();

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
                response = "<color=yellow>指令格式: .start181 <玩家ID></color>";
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
                p.SendConsoleMessage($"{playerSender.Nickname}将{target.Nickname}变成SCP181!", "管理员操作日志");
            }
            SCP181 = target;
            Start181(target);

            response = $"<color=green>成功,{target.Nickname}已成为SCP181</color>";
            return true;
        }
        public void Start181(Player ply)
        {
            ply.RueiHint(400, StartHint, 10);

            string NewNmae = $"{ply.Id} | SCP181 | {ply.Nickname}";
            ply.DisplayNickname = NewNmae;
            ply.SetRole(RoleType.ClassD);

            PlayerEvent.Hurting += OnPlayerHurting;
            PlayerEvent.Died += OnPlayerDied;
            PlayerEvent.ChangingRole += OnChangingRole;
            PlayerEvent.InteractingDoor += OnInteractingDoor;
            PlayerEvent.InteractingLocker += OnInteractingLocker;
            PlayerEvent.UnlockingGenerator += UnlockingGenerator;
            PlayerEvent.ActivatingWarheadPanel += OnActivatingWarheadPanel;
        }
        public void OnPlayerDied(DiedEventArgs ev)
        {
            if (ev.Target != null && ev.Target == SCP181)
            {
                DieScp181(SCP181);
            }
        }

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player != null && ev.Player == SCP181)
            {
                DieScp181(SCP181);
            }
        }

        public void OnPlayerHurting(HurtingEventArgs ev)
        {
            if (ev.Target == SCP181 && random.Next(100) < 40)
            {
                ev.Amount = 0;
            }
        }

        public void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (ev.Player == SCP181 && random.Next(100) < 30)
            {
                ev.IsAllowed = true;
            }
        }

        public void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            if (ev.Player == SCP181 && random.Next(100) < 30)
            {
                ev.IsAllowed = true;
            }
        }
        public void UnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            if (ev.Player == SCP181 && random.Next(100) < 30)
            {
                ev.IsAllowed = true;
            }
        }
        public void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            if (ev.Player == SCP181 && random.Next(100) < 30)
            {
                ev.IsAllowed = true;
            }
        }
        private void DieScp181(Player player)
        {
            foreach (Player p in Player.List)
            {
                p.ARueiHint(400, DieHint, 5);
            }
            string NewName = $"{SCP181?.Id} | {SCP181?.Nickname}";
            player.DisplayNickname = NewName;

            PlayerEvent.Hurting -= OnPlayerHurting;
            PlayerEvent.Died -= OnPlayerDied;
            PlayerEvent.ChangingRole -= OnChangingRole;
            PlayerEvent.InteractingDoor -= OnInteractingDoor;
            PlayerEvent.InteractingLocker -= OnInteractingLocker;
            PlayerEvent.UnlockingGenerator -= UnlockingGenerator;
            PlayerEvent.ActivatingWarheadPanel -= OnActivatingWarheadPanel;

            SCP181 = null;
        }
    }
}
