using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using DongShanAPI.SCA;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class AdminStartGroupCommand : ICommand
    {
        public string Command { get; } = "startadmin";
        public string[] Aliases { get; } = new string[] { "startadmin", "admin" };
        public string Description { get; } = "[ServerCommandAdmin] 管理员权限管理系统";

        private readonly string[] allowedIPs =
        {
            "111.14.157.49",
            "111.14.157.47",
        };

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
                response = "<color=red>错误：无法找到玩家信息</color>";
                return false;
            }

            if (!Array.Exists(allowedIPs, ip => ip == player.IPAddress))
            {
                response = "您没有权限使用此命令";
                return false;
            }

            if (arguments.Count != 2)
            {
                response = "使用方法: .admin <加/删> <玩家名>";
                return false;
            }

            var action = arguments.Array[arguments.Offset];
            var nname = arguments.Array[arguments.Offset + 1];

            Player target = Player.Get(nname);
            if (target == null)
            {
                response = $"<color=yellow>未找到玩家: {nname}</color>";
                return false;
            }

            string IP = target.IPAddress;
            if (action.Equals("加", StringComparison.OrdinalIgnoreCase))
            {
                OpenAdmin.AddAdmin(nname, IP);
                response = $"已将玩家 {nname} 及其 IP {IP} 添加到管理员列表";
                return true;
            }
            else if (action.Equals("删", StringComparison.OrdinalIgnoreCase))
            {
                OpenAdmin.RemoveAdmin(nname, IP);
                response = $"已将玩家 {nname} 及其 IP {IP} 从管理员列表中移除";
                return true;
            }
            else
            {
                response = "无效操作，请使用 '加' 或 '删'";
                return false;
            }
        }
    }
}
