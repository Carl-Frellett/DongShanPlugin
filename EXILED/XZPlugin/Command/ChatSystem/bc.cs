using CommandSystem;
using DongShanAPI.Hint;
using Exiled.API.Features;
using RemoteAdmin;
using System;

namespace Chat
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ChatBCommand : ICommand
    {
        public string Command { get; } = "bc";
        public string[] Aliases { get; } = new string[] { };
        public string Description { get; } = "全体消息";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender playerSender)
            {
                // 获取执行命令的玩家实例
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

                // 拼接消息内容
                string message = string.Join(" ", arguments);
                string teamName = GetTeamName(player.Team); //团队调用

                string bc = $"<align=left><size=25><color=#c3a938ff>[全体消息]</color> {teamName} [{player.Nickname}]:<color=white> {message}</color></size></align>";

                foreach (Player p in Player.List)
                {
                    p.ARueiHint(500, bc, 5);
                    p.SendConsoleMessage($"[BC] 玩家{player.Nickname}发送了全体消息: {message}", "聊天系统");
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

        //团队判断
        private string GetTeamName(Team team)
        {
            switch (team)
            {
                case Team.SCP:
                    return "<color=red>[SCP]</color>";
                case Team.MTF:
                    return "<color=blue>[九尾狐]</color>";
                case Team.CHI:
                    return "<color=green>[混沌分裂者]</color>";
                case Team.RSC:
                    return "<color=yellow>[科学家]</color>";
                case Team.CDP:
                    return "<color=orange>[D级人员]</color>";
                case Team.TUT:
                    return "<color=purple>[教程角色]</color>";
                default:
                    return "[观察者]";
            }
        }
    }
}
