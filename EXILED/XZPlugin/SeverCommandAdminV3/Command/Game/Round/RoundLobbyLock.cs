using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using DongShanAPI.SCA;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class RoundLobbyLockCommand : ICommand
    {
        public string Command { get; } = "locklobbyround";
        public string[] Aliases { get; } = { "llr" };
        public string Description { get; } = "[ServerCommandAdmin] 锁局";

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

            foreach (Player p in Player.List)
            {
                p.SendConsoleMessage($"{playerSender.Nickname}使用了锁局指令!", "管理员操作日志");
            }
            Round.IsLobbyLocked = !Round.IsLobbyLocked;

            response = Round.IsLocked ? "<color=green>大厅已锁定</color>" : "<color=green>大厅已解锁</color>";

            return true;
        }
    }
}
