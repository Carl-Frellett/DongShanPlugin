using System;
using CommandSystem;
using DongShanAPI.Hint;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using RemoteAdmin;

namespace XZPlugin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class TX : ICommand
    {
        public string Command { get; } = "tx";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "投降";

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
            if (player.Role == RoleType.ClassD && player.IsAlive == true)
            {
                if (player.RankName == "")
                {
                    player.RankName = "投降人员";
                    response = "投降成功！";
                    return true;
                }
                else
                {
                    player.RankName += " | 投降人员";
                    response = "投降成功！";
                    return true;
                }
            }

            else if (player.Role != RoleType.ClassD)
            {
                response = "投降失败！你不是D级人员";
                return false;
            }
            else if (player.IsAlive != true)
            {
                response = "投降失败！你死了";
                return false;
            }
            else if (player.RankName.Contains("投降人员"))
            {
                response = "投降失败！已经投降了";
                return false;
            }
            else
            {
                response = "投降失败！未知原因！";
                return false;
            }
        }
    }
}
