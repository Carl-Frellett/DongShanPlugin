using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using DongShanAPI.SCA;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class BanCommand : ICommand
    {
        public string Command { get; } = "ban";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "[ServerCommandAdmin] 封禁玩家";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count < 3)
            {
                response = "<color=yellow>格式: ban <玩家ID> <时间> <原因></color>";
                return false;
            }

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
                response = "<color=red>错误：你没有权限使用这个命令</color>";
                return false;
            }

            if (!player.ReferenceHub.serverRoles.RemoteAdmin)
            {
                response = "<color=red>错误：未经过管理员验证!</color>";
                return false;
            }

            string playerID = arguments.At(0);
            Player target = Player.Get(playerID);
            if (target == null)
            {
                response = "<color=red>错误：找不到玩家</color>";
                return false;
            }

            if (!int.TryParse(arguments.At(1), out int duration))
            {
                response = "<color=red>错误：无效的时间</color>";
                return false;
            }

            string reason = string.Join(" ", arguments.Array, 2, arguments.Count - 2);

            // 执行封禁操作
            target.Ban(duration, reason, sender.LogName);

            response = $"<color=green>玩家{target.Nickname} 已被封禁 {duration} 分钟, 原因：{reason}</color>";

            foreach (Player p in Player.List)
            {
                p.SendConsoleMessage($"{playerSender.Nickname}使用了封禁指令!", "管理员操作日志");
            }

            return true;
        }
    }
}
