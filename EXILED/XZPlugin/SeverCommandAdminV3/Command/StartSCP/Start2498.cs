using CommandSystem;
using DongShanAPI.Hint;
using Exiled.Events.EventArgs;
using RemoteAdmin;
using System;
using Player = Exiled.API.Features.Player;
using DongShanAPI.SCA;
using PlayerEvent = Exiled.Events.Handlers.Player;
using MEC;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Start2498Command : ICommand
    {
        public string Command { get; } = "start2498";
        public string[] Aliases { get; } = new string[] { };
        public string Description { get; } = "[ServerCommandAdmin] 指定某一名玩家成为2498";

        public Player SCP2498;
        string DieHint = "<align=left><color=yellow><size=25>[收容通知]<color=red> SCP-2498现已被收容</color></size></align>";
        string StartHint = "<b>你是\n<size=100><color=red>SCP-2498</color></size>\n透视</b>";

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
                response = "<color=yellow>指令格式: .start2498 <玩家ID></color>";
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
                p.SendConsoleMessage($"{playerSender.Nickname}将{target.Nickname}变成SCP2498!", "管理员操作日志");
            }
            SCP2498 = target;
            Start2498(target);

            response = $"<color=green>成功,{target.Nickname}已成为SCP2498</color>";
            return true;
        }
        public void Start2498(Player player)
        {
            player.RueiHint(400, StartHint, 10);

            string NewName = $"{SCP2498.Id} | SCP2498 | {SCP2498.Nickname}";
            player.DisplayNickname = NewName;
            player.SetRole(RoleType.ClassD);

            Timing.CallDelayed(0.5f, () => player.ReferenceHub.playerEffectsController.EnableEffect<CustomPlayerEffects.Visuals939>());

            PlayerEvent.Died += OnPlayerDied;
            PlayerEvent.ChangingRole += OnChangingRole;
        }
        public void OnPlayerDied(DiedEventArgs ev)
        {
            if (ev.Target == SCP2498 && SCP2498 != null)
            {
                DieSCP2498(SCP2498);
            }
        }

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player == SCP2498 && SCP2498 != null)
            {
                DieSCP2498(SCP2498);
            }
        }

        private void DieSCP2498(Player player)
        {
            player.ReferenceHub.playerEffectsController.DisableEffect<CustomPlayerEffects.Visuals939>();

            string NewName = $"{SCP2498.Id} | {SCP2498.Nickname}";
            player.DisplayNickname = NewName;

            foreach (Player p in Player.List)
            {
                p.ARueiHint(300, DieHint, 5);
            }
            PlayerEvent.Died -= OnPlayerDied;
            PlayerEvent.ChangingRole -= OnChangingRole;

            SCP2498 = null;
        }
    }
}
