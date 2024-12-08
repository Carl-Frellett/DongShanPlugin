using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using DongShanAPI.SCA;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class BypassCommand : ICommand
    {
        public string Command { get; } = "bypass";
        public string[] Aliases { get; } = new string[] { "by" };
        public string Description { get; } = "[ServerCommandAdmin] 金手指";

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
                ToggleBypass(player);
                response = responses(player);
                return true;
            }

            string target = arguments.At(0);
            if (target.ToLower() == "all")
            {
                foreach (Player p in Player.List)
                {
                    ToggleBypass(p);
                }

                response = "<color=green>所有玩家均已获得bypass权限</color>";
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

            ToggleBypass(oPlayer);
            response = responses(oPlayer);
            foreach (Player p in Player.List)
            {
                p.SendConsoleMessage($"{playerSender.Nickname}使用了金手指指令!", "管理员操作日志");
            }
            return true;
        }

        private void ToggleBypass(Player player)
        {
            player.IsBypassModeEnabled = !player.IsBypassModeEnabled;
        }

        private string responses(Player player)
        {
            return $"\n<color=green>权限使用者：{player.Nickname}</color>\n<color=green>Bypass状态：<color=yellow>{(player.IsBypassModeEnabled ? "启用" : "禁用")}</color></color>";
        }
    }
}
