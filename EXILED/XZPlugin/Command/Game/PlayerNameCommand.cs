using CommandSystem;
using Exiled.API.Features;
using MEC;
using RemoteAdmin;
using System;

namespace XZPlugin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class PlayerNameCommand : ICommand
    {
        public string Command { get; } = "pi";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "显示自己的名称";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender is PlayerCommandSender playerSender))
            {
                response = "此命令只能由玩家使用";
                return false;
            }

            Player player = Player.Get(playerSender.Nickname);
            if (player == null)
            {
                response = "错误：找不到命令使用者";
                return false;
            }

            string SafeName = string.IsNullOrWhiteSpace(player.Nickname) || player.Nickname.Equals("Player") ? $"Player {player.IPAddress}" : player.Nickname;
            string NewName = $"{player.Id} | {SafeName}";
            player.DisplayNickname = NewName;

            response = $"执行完毕，您的名称已修复";

            return true;
        }
    }
}
