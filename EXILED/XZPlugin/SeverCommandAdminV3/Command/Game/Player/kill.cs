using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using DongShanAPI.SCA;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class KillPlayerCommand : ICommand
    {
        public string Command { get; } = "kill";
        public string[] Aliases { get; } = new string[] { };
        public string Description { get; } = "[ServerCommandAdmin] 强制杀死某一名玩家";

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
                response = "<color=yellow>指令格式: .kill <玩家ID></color>";
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
                p.SendConsoleMessage($"{playerSender.Nickname}使用了Kill指令!", "管理员操作日志");
            }
            target.Kill();
            response = $"<color=green>玩家：{target.Nickname} 已被杀死</color>";
            return true;
        }
    }
}