using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using DongShanAPI.SCA;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class NukeCommand : ICommand
    {
        public string Command { get; } = "nuk";
        public string[] Aliases { get; } = new string[] { };
        public string Description { get; } = "[ServerCommandAdmin] 核弹控制";

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

            if (arguments.Count != 1)
            {
                response = "<color=yellow>指令格式: .nuk <st/sp/de></color>";
                return false;
            }

            foreach (Player p in Player.List)
            {
                p.SendConsoleMessage($"{playerSender.Nickname}使用了核弹指令!", "管理员操作日志");
            }

            string option = arguments.At(0).ToLower();
            switch (option)
            {
                case "st":
                    Warhead.Start();
                    response = "<color=green>核弹已启动</color>";
                    return true;

                case "sp":
                    Warhead.Stop();
                    response = "<color=green>核弹已停止</color>";
                    return true;

                case "de":
                    Warhead.Detonate();
                    response = "<color=green>核弹已准备引爆</color>";
                    return true;

                default:
                    response = "<color=red>无效的命令请使用 st, sp 或 de</color>";
                    return false;
            }
        }
    }
}
