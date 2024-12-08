using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using DongShanAPI.SCA;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class SetPlayerSizeCommand : ICommand
    {
        public string Command { get; } = "size";
        public string[] Aliases { get; } = new string[] { };
        public string Description { get; } = "[ServerCommandAdmin] 修改体积";

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

            if (arguments.Count < 4)
            {
                response = "<color=red>错误：参数数量不正确。用法: .size <玩家ID或all> <X体积> <Y体积> <Z体积></color>";
                return false;
            }

            string targeted = arguments.At(0);
            if (!float.TryParse(arguments.At(1), out float X) || !float.TryParse(arguments.At(2), out float Y) || !float.TryParse(arguments.At(3), out float Z))
            {
                response = "<color=red>错误：体积参数必须为数字</color>";
                return false;
            }

            if (targeted.ToLower() == "all")
            {
                foreach (Player p in Player.List)
                {
                    SetPlayerSize(p, X, Y, Z);
                }
                response = "<color=green>成功：已修改所有玩家的体积</color>";
            }
            else
            {
                if (!int.TryParse(targeted, out int playerId))
                {
                    response = "<color=red>错误：无效的玩家ID或命令参数</color>";
                    return false;
                }

                Player oPlayer = Player.Get(playerId);
                if (oPlayer == null)
                {
                    response = "<color=red>错误：找不到指定ID的玩家</color>";
                    return false;
                }

                SetPlayerSize(oPlayer, X, Y, Z);
                response = $"<color=green>成功：已修改玩家 {oPlayer.Nickname} 的体积</color>";
            }

            foreach (Player p in Player.List)
            {
                p.SendConsoleMessage($"{playerSender.Nickname}使用了体积指令!", "管理员操作日志");
            }
            return true;
        }

        private void SetPlayerSize(Player player, float X, float Y, float Z)
        {
            player.Scale = new UnityEngine.Vector3(X, Y, Z);
        }
    }
}