using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using DongShanAPI.SCA;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class OverwatchCommand : ICommand
    {
        public string Command { get; } = "overwatch";
        public string[] Aliases { get; } = new string[] { "ow" };
        public string Description { get; } = "[ServerCommandAdmin] 监管模式";

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
                p.SendConsoleMessage($"{playerSender.Nickname}使用了监管指令!", "管理员操作日志");
            }

            if (arguments.Count == 0)
            {
                ToggleOverwatchMode(player);
                response = FormResponse(player);
                return true;
            }

            var targetIdOrAll = arguments.At(0).ToLower();

            if (targetIdOrAll == "all")
            {
                foreach (Player p in Player.List)
                {
                    ToggleOverwatchMode(p);
                }

                response = "<color=green>所有玩家均已获得Godmode权限</color>";
                return true;
            }

            if (!uint.TryParse(targetIdOrAll, out uint playerId))
            {
                response = "<color=red>错误：无效的参数</color>";
                return false;
            }

            var targetPlayer = Player.Get((int)playerId);
            if (targetPlayer == null)
            {
                response = $"<color=red>错误：未找到ID为{playerId}的玩家</color>";
                return false;
            }

            ToggleOverwatchMode(targetPlayer);
            response = FormResponse(targetPlayer);

            return true;
        }

        private void ToggleOverwatchMode(Player player)
        {
            player.IsOverwatchEnabled = !player.IsOverwatchEnabled; // 切换玩家的Overwatch状态
        }

        private string FormResponse(Player player)
        {
            return $"\n<color=green>权限使用者：{player.Nickname}</color>\n<color=green>Overwatch状态：<color=yellow>{(player.IsOverwatchEnabled ? "启用" : "禁用")}</color></color>";
        }
    }
}
