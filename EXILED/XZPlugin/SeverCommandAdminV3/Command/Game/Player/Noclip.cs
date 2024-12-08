using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using DongShanAPI.SCA;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class NoclipCommand : ICommand
    {
        public string Command { get; } = "noclip";
        public string[] Aliases { get; } = new string[] { "np" };
        public string Description { get; } = "[ServerCommandAdmin] 飞行";

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
                response = "<color=red>错误：你没有管理员权限</色>";
                return false;
            }

            if (!player.ReferenceHub.serverRoles.RemoteAdmin)
            {
                response = "<color=red>错误：未经过管理员验证!</color>";
                return false;
            }
            foreach (Player p in Player.List)
            {
                p.SendConsoleMessage($"{playerSender.Nickname}使用了飞行指令!", "管理员操作日志");
            }
            if (arguments.Count == 0)
            {
                ToggleNoclip(player);
                response = FormResponse(player);
                return true;
            }
            else if (arguments.Count == 1)
            {
                string targetId = arguments.At(0).ToLower();
                if (targetId == "all")
                {
                    foreach (Player p in Player.List)
                    {
                        ToggleNoclip(p);
                    }
                    response = "<color=green>所有玩家均已获得Noclip权限</color>";
                    return true;
                }
                else if (uint.TryParse(targetId, out uint playerId))
                {
                    Player target = Player.Get((int)playerId);
                    if (target == null)
                    {
                        response = $"<color=red>错误：无法找到ID为 {playerId} 的玩家</color>";
                        return false;
                    }

                    ToggleNoclip(target);
                    response = FormResponse(target);
                    return true;
                }
                else
                {
                    response = "<color=red>错误：无效的参数</color>";
                    return false;
                }
            }

            else
            {
                response = "<color=yellow>格式: .np [玩家ID/all]</color>";
                return false;
            }

        }

        private void ToggleNoclip(Player player)
        {
            player.NoClipEnabled = !player.NoClipEnabled;
        }

        private string FormResponse(Player player)
        {
            return $"\n<color=green>权限使用者：{player.Nickname}</color>\n<color=green>Noclip状态：<color=yellow>{(player.NoClipEnabled ? "启用" : "禁用")}</color></color>";

        }
    }
}
