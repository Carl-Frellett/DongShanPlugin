using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using DongShanAPI.SCA;

namespace XZPlugin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class SverinfoCommand : ICommand
    {
        public string Command { get; } = "p";
        public string[] Aliases { get; } = Array.Empty<string>();
        public string Description { get; } = "查询服务器的基本信息";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string sein = $"<color=green>{Plugin.Instance.ServerName}</color>"; //服务器名字

            string gv = $"<color=green>{Plugin.Instance.Ver_Game}</color>"; //游戏版本

            string pv = $"<color=Green>XZPlugin v{Plugin.Instance.Version} ({Plugin.Instance.Ver_API})</color>"; //插件版本

            string ld = "<color=green>\nEXILED 2.1.14 (插件框架)\nRuel 3.0.0 (信息显示框架)</color></color>"; //框架版本

            if (sender is PlayerCommandSender playerSender)
            {
                Player player = Player.Get(playerSender.Nickname);
                if (player != null)
                {
                    bool isAdmin = AdminFileManager.IsAdmin(player.Nickname, player.IPAddress);

                    string Rankname = player.Group != null ? player.Group.BadgeText : "None";
                    string Rankcolor = player.RankColor != null ? player.RankColor : "None";

                    response =
                        $"" +
                        //服务器信息
                        $"\n<color=yellow># 服务器信息</color>" +
                        $"\n<color=white>服务器名称：{sein}" +
                        $"\n<color=White>运行端口：{ServerStatic.ServerPort}" +
                        $"\n<color=White>游戏版本：{gv}" +
                        $"\n<color=White>插件版本：{pv}" +
                        $"\n<color=White>框架版本：{ld}" +
                        //玩家信息
                        $"\n\n\n<color=yellow># 玩家信息</color>" +
                        $"\n<color=White>玩家名称：</color><color=green>{player.Nickname}</color>" +
                        $"\n<color=White>玩家IP：</color><color=green>{player.IPAddress}</color>" +
                        $"\n<color=White>称号：</color><color={Rankcolor}>{Rankname}</color>" +
                        $"\n<color=White>管理员权限：</color><color=green>{(isAdmin ? "是" : "否")}</color>" +
                        //欢迎消息
                        $"\n\n<color=blue>欢迎游玩<color=yellow>東山</color>怀旧服务器, <color=red>加入我们的Q群：715253424</color></color>";
                    return true;
                }
            }

            response = "<color=red>错误：无法获取玩家信息</color>";
            return false;
        }
    }
}
