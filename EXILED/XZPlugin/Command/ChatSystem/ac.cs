using CommandSystem;
using DongShanAPI.Hint;
using DongShanAPI.SCA;
using Exiled.API.Features;
using RemoteAdmin;
using System;

namespace Chat
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ACommand : ICommand
    {
        public string Command { get; } = "ac";
        public string[] Aliases { get; } = new string[] { };
        public string Description { get; } = "管理求助";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender playerSender)
            {
                Player player = Player.Get(playerSender.Nickname);

                if (player == null)
                {
                    response = "<color=red>错误：未找到此玩家</color>";
                    return false;
                }

                if (arguments.Count == 0)
                {
                    response = "<color=red>请输入内容</color>";
                    return false;
                }

                string message = string.Join(" ", arguments);
                bool isAdmin = AdminFileManager.IsAdmin(player.Nickname, player.IPAddress);

                string bc = $"<align=left><b><size=25><color=red>[管理员求助]</color>[{player.Nickname}]:<color=white> {message}</color></size></b></align>";

                foreach (Player p in Player.List)
                {
                    if (isAdmin == true)
                    {
                        p.ARueiHint(500, bc, 5);
                    }
                    p.SendConsoleMessage($"[BC] 玩家{player.Nickname}发送了管理求助: {message}", "管理求助");
                    response = bc;
                }
                global::ServerConsole.AddLog($"[BC] 玩家{player.Nickname}发送了全体消息: {message}", ConsoleColor.Cyan);

                response = "<color=green>发送成功</color>";
                return true;
            }
            else
            {
                response = "<color=red>错误：命令只能由玩家使用</color>";
                return false;
            }
        }
    }
}
