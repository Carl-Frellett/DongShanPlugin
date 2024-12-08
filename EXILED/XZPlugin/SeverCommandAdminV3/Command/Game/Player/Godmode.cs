using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using DongShanAPI.SCA;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class GodmodeCommand : ICommand
    {
        public string Command { get; } = "godmode";
        public string[] Aliases { get; } = new string[] { "gd" };
        public string Description { get; } = "[ServerCommandAdmin] 上帝模式(无敌)";

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

            if (arguments.Count == 0)
            {
                ToggleGodMode(player);
                response = FormResponse(player);
                return true;
            }

            var target = arguments.At(0).ToLower();

            if (target == "all")
            {
                foreach (Player p in Player.List)
                {
                    ToggleGodMode(p);
                }

                response = "<color=green>所有玩家均已获得Godmode权限</color>";
                return true;
            }

            if (!uint.TryParse(target, out uint playerId))
            {
                response = "<color=red>错误：无效的参数</color>";
                return false;
            }

            var oPlayer = Player.Get((int)playerId);

            if (oPlayer == null)
            {
                response = $"<color=red>错误：未找到ID为{playerId}的玩家</color>";
                return false;
            }

            ToggleGodMode(oPlayer);

            response = FormResponse(oPlayer);
            foreach (Player p in Player.List)
            {
                p.SendConsoleMessage($"{playerSender.Nickname}使用了GodMod指令!", "管理员操作日志");
            }
            return true;
        }

        private void ToggleGodMode(Player player)
        {
            player.IsGodModeEnabled = !player.IsGodModeEnabled;
        }

        private string FormResponse(Player player)
        {
            return $"\n<color=green>权限使用者：{player.Nickname}</color>\n<color=green>GodMode状态：<color=yellow>{(player.IsGodModeEnabled ? "启用" : "禁用")}</color></color>";
        }
    }
}
