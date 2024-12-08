using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using DongShanAPI.SCA;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ChangeRoleCommand : ICommand
    {
        public string Command { get; } = "fcp";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "[ServerCommandAdmin] 更改玩家的游戏角色";

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

            if (arguments.Count < 2 || arguments.Count > 3)
            {
                response = "<color=yellow>指令格式: .fcp <玩家ID/all> <角色ID> <是否保留玩家的位置与物品(y/n)></color>";
                return false;
            }

            string target = arguments.At(0).ToLower();

            if (!int.TryParse(arguments.At(1), out int roleId))
            {
                response = "<color=red>错误：角色ID必须为整数</color>";
                return false;
            }

            RoleType role = (RoleType)roleId;

            if (!Enum.IsDefined(typeof(RoleType), role))
            {
                response = $"<color=red>无效的游戏角色ID: {roleId}</color>";
                return false;
            }

            bool Islite = false;

            if (arguments.Count == 3)
            {
                switch (arguments.At(2).ToLower())
                {
                    case "y":
                        Islite = true;
                        break;
                    case "n":
                        Islite = false;
                        break;
                    default:
                        response = "<color=red>该选项只能是'y'或'n'</color>";
                        return false;
                }
            }

            if (target == "all")
            {
                foreach (Player p in Player.List)
                {
                    p.SetRole(role, Islite);
                }
                response = $"<color=green>所有玩家的角色已更改为 {role}</color>";
                return true;
            }

            if (!int.TryParse(target, out int oPlayerId))
            {
                response = "<color=red>错误：无效的玩家ID</color>";
                return false;
            }

            Player oPlayer = Player.Get(oPlayerId);
            if (oPlayer == null)
            {
                response = $"<color=red>错误：未找到ID为 {oPlayerId} 的玩家</color>";
                return false;
            }

            foreach (Player p in Player.List)
            {
                p.SendConsoleMessage($"{playerSender.Nickname}使用了玩家角色更改指令!", "管理员操作日志");
            }

            oPlayer.SetRole(role, Islite);
            response = $"<color=green> 玩家{oPlayer.Nickname} 的游戏角色已更改为{role}</color> ";
            return true;
        }
    }
}
