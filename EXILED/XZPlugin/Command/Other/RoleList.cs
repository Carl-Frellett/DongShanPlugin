using CommandSystem;
using System;
using System.Collections.Generic;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class RoleListCommand : ICommand
    {
        public string Command { get; } = "RoleList";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "角色列表";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 0)
            {
                response =
                    "\nSCP-173 - 0" +
                    "\nD级人员 - 1" +
                    "\n旁观者 - 2" +
                    "\nSCP-106 - 3" +
                    "\n收容专家 - 4" +
                    "\nSCP-049 - 5" +
                    "\n科学家 - 6" +
                    "\nSCP-079 - 7" +
                    "\n混沌 - 8" +
                    "\nSCP-096 - 9" +
                    "\nSCP-0492 - 10" +
                    "\n中士 - 11" +
                    "\n指挥官 - 12" +
                    "\n新兵 - 13" +
                    "\n训练人员 - 14" +
                    "\n设施警卫 - 15" +
                    "\nSCP-939-53 - 16" +
                    "\nSCP-939-89 - 17";
                return true;
            }

            response = "<color=red>错误: 无效的指令</color>";
            return false;
        }
    }
}
