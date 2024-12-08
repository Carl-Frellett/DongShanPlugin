using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;

namespace XZPlugin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class KillmeCommand : ICommand
    {
        public string Command { get; } = "killme";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "自杀";

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

            player.Hurt(999999999999);

            response = $"自杀成功";

            return true;
        }
    }
}
