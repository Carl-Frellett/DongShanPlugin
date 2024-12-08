using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using DongShanAPI.SCA;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class KickCommand : ICommand
    {
        public string Command { get; } = "kick";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "[ServerCommandAdmin] 踢出玩家";

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

            // 检查命令使用者是否为管理员
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

            if (arguments.Count < 2)
            {
                response = "<color=yellow>格式: kick <玩家ID|玩家昵称> <原因></color>";
                return false;
            }

            string targetIdentifier = arguments.At(0);
            Player target = Player.Get(targetIdentifier);
            if (target == null)
            {
                response = $"<color=red>错误：找不到玩家 {targetIdentifier}</color>";
                return false;
            }
            foreach (Player p in Player.List)
            {
                p.SendConsoleMessage($"{playerSender.Nickname}使用了玩家踢出指令!", "管理员操作日志");
            }
            string reason = string.Join(" ", arguments.Array, 1, arguments.Count - 1);
            target.Kick(reason);
            response = $"<color=green>玩家 {target.Nickname} 已被踢出服务器, 原因：{reason}</color>";
            return true;
        }
    }
}
