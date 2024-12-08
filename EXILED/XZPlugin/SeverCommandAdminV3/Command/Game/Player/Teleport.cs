using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using DongShanAPI.SCA;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class TeleportCommand : ICommand
    {
        public string Command { get; } = "tp";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "[ServerCommandAdmin] 传送玩家到另一玩家的位置";

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

            if (arguments.Count != 2)
            {
                response = "<color=yellow>格式: .tp <玩家ID或'all'> <玩家ID></color>";
                return false;
            }

            string targetId = arguments.At(1);
            if (targetId.ToLower() == "all")
            {
                response = "<color=red>错误：第二个参数不能为'all'</color>";
                return false;
            }

            Player targetPlayer = Player.Get(targetId);
            if (targetPlayer == null)
            {
                response = "<color=red>错误：目标玩家ID无效</color>";
                return false;
            }
            foreach (Player p in Player.List)
            {
                p.SendConsoleMessage($"{playerSender.Nickname}使用了传送指令!", "管理员操作日志");
            }

            if (arguments.At(0).ToLower() == "all")
            {
                foreach (Player p in Player.List)
                {
                    p.Position = targetPlayer.Position;
                }
                response = $"<color=green>所有玩家已被传送到 {targetPlayer.Nickname} 的位置</color>";
                return true;
            }
            else
            {
                Player player1 = Player.Get(arguments.At(0));
                if (player1 == null)
                {
                    response = "<color=red>错误：指定的玩家ID无效</color>";
                    return false;
                }

                player1.Position = targetPlayer.Position;
                response = $"<color=green>{player1.Nickname} 已被传送到 {targetPlayer.Nickname} 的位置</color>";
                return true;
            }
        }
    }
}
