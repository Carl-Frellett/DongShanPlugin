using CommandSystem;
using DongShanAPI.SCA;
using Exiled.API.Features;
using RemoteAdmin;
using System;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class AdminGroupCommand : ICommand
    {
        public string Command { get; } = "ad";
        public string[] Aliases { get; } = Array.Empty<string>();
        public string Description { get; } = "[ServerCommandAdmin] 管理员自助验证系统";

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
                response = "<color=red>错误：你不能使用此权限</color>";
                return false;
            }

            UserGroup group = ServerStatic.GetPermissionsHandler().GetGroup("admin");
            if (group != null)
            {
                player.Group = group;
                response = "<color=green>验证通过! </color>";
                return true;
            }

            response = "<color=red>无法执行验证操作！原因：无法读取必要的验证文件</color>";
            return false;
        }

    }
}
