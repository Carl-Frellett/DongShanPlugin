using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;

namespace Chat
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ChatCommand : ICommand
    {
        public string Command { get; } = "c";
        public string[] Aliases { get; } = new string[] { };
        public string Description { get; } = "团队消息";

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
                string teamName = GetTeamName(player.Team);

                foreach (var Teams in Player.List)
                {
                    if (Teams.Team == player.Team)
                    {
                        Teams.Broadcast(5, $"<size=30><color=#c3a938ff>[团队]</color> {teamName} [{player.Nickname}]:<color=white> {message}</color></size>", Broadcast.BroadcastFlags.Normal);
                    }
                }
                response = "<color=green>发送成功</color>";
                return true;
            }
            else
            {
                response = "<color=red>错误：未知原因</color>";
                return false;
            }
        }
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
