using CommandSystem;
using DongShanAPI.Hint;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using System.IO;
using System.Linq;

namespace Chat
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class QCommand : ICommand
    {
        public string Command { get; } = "q";
        public string[] Aliases { get; } = new string[] { };
        public string Description { get; } = "悄悄话系统";

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

                if (arguments.Count < 3)
                {
                    response = "<color=red>使用方式：.q 玩家ID 消息 y/n（是否匿名）</color>";
                    return false;
                }

                if (!int.TryParse(arguments.At(0), out int targetId))
                {
                    response = "<color=red>错误：玩家ID必须是数字</color>";
                    return false;
                }

                Player target = Player.Get(targetId);
                if (target == null)
                {
                    response = "<color=red>错误：未找到目标玩家</color>";
                    return false;
                }

                bool isAnonymous = arguments.At(arguments.Count - 1).ToLower() == "y";
                string message = string.Join(" ", arguments.Skip(1).Take(arguments.Count - 2)); // 纠正的合并消息部分

                bool isAdmin = IsAdmin(player.Nickname, player.IPAddress);
                string adminTag = isAdmin ? "(<color=yellow>管理员</color>)" : "";
                string senderName = isAnonymous ? "有人" : $"{player.Nickname} {adminTag}";
                target.ARueiHint(350, $"<size=25><b>{senderName}悄悄的对你说</b>: <i>{message}</i></size>", 5);
                response = $"你悄悄的对{target.Nickname}说: {message}";
                return true;
            }
            else
            {
                response = "<color=red>错误：命令只能由玩家使用</color>";
                return false;
            }
        }

        private bool IsAdmin(string username, string ipAddress)
        {
            string adminFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SCP Secret Laboratory", "ServerAdmin.txt");
            if (File.Exists(adminFilePath))
            {
                var adminData = File.ReadAllLines(adminFilePath);
                return adminData.Any(line => line.Contains(username) || line.Contains(ipAddress));
            }
            return false;
        }
    }
}
