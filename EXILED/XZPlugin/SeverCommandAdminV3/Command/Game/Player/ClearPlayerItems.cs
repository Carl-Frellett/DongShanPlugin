using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using DongShanAPI.SCA;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ClearPlayerItemsCommand : ICommand
    {
        public string Command { get; } = "cin";
        public string[] Aliases { get; } = new string[] { };
        public string Description { get; } = "[ServerCommandAdmin] 清理玩家背包物品";

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

            if (!AdminFileManager.IsAdmin(player.Nickname, player.IPAddress))
            {
                response = "<color=red>错误：你没有权限使用这个命令</color>";
                return false;
            }

            if (!player.ReferenceHub.serverRoles.RemoteAdmin)
            {
                response = "<color=red>错误：未经过管理员验证!</color>";
                return false;
            }

            if (arguments.Count == 0)
            {
                player.ClearInventory();
                response = $"<color=green>已清除 {player.Nickname} 的背包物品</color>";
                return true;
            }

            string target = arguments.At(0);
            if (target.ToLower() == "all")
            {
                foreach (Player p in Player.List)
                {
                    p.ClearInventory();
                }
                response = "<color=green>已清除所有玩家的背包物品</color>";
                return true;
            }

            if (!uint.TryParse(target, out uint playerId))
            {
                response = "<color=red>错误：无效的玩家ID</color>";
                return false;
            }

            Player oPlayer = Player.Get((int)playerId);
            if (oPlayer == null)
            {
                response = $"<color=red>错误：无法找到ID为 {playerId} 的玩家</color>";
                return false;
            }

            oPlayer.ClearInventory();
            response = $"<color=green>已清除 {oPlayer.Nickname} 的背包物品</color>";
            foreach (Player p in Player.List)
            {
                p.SendConsoleMessage($"{playerSender.Nickname}使用了清理垃圾指令!", "管理员操作日志");
            }
            return true;
        }
    }
}
