using CommandSystem;
using System;
using System.Collections.Generic;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class ItemListCommand : ICommand
    {
        public string Command { get; } = "ItemList";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "物品列表";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 0)
            {
                response =
                    "\n钥匙卡清洁工 - 0" +
                    "\n钥匙卡科学家 - 1" +
                    "\n主要科学家钥匙卡 - 2" +
                    "\n区域经理钥匙卡 - 3" +
                    "\n警卫钥匙卡 - 4" +
                    "\n高级警卫钥匙卡 - 5" +
                    "\n containmentEngineer钥匙卡 - 6" +
                    "\nNTF中尉钥匙卡 - 7" +
                    "\nNTF指挥官钥匙卡 - 8" +
                    "\n设施经理钥匙卡 - 9" +
                    "\n混沌叛乱钥匙卡 - 10" +
                    "\nO5钥匙卡 - 11" +
                    "\n对讲机 - 12" +
                    "\nCOM15手枪 - 13" +
                    "\n急救包 - 14" +
                    "\n手电筒 - 15" +
                    "\n微型HID - 16" +
                    "\nSCP500 - 17" +
                    "\nSCP207 - 18" +
                    "\n武器管理面板 - 19" +
                    "\nE11-SR手枪 - 20" +
                    "\nProject90手枪 - 21" +
                    "\n.556子弹 - 22" +
                    "\nMP7手枪 - 23" +
                    "\nLogicer手枪 - 24" +
                    "\n破片手雷 - 25" +
                    "\n闪光手雷 - 26" +
                    "\n缴械器 - 27" +
                    "\n.762子弹 - 28" +
                    "\n9mm子弹 - 29" +
                    "\nUSP手枪 - 30" +
                    "\nSCP018 - 31" +
                    "\nSCP268 - 32" +
                    "\n肾上腺素 - 33" +
                    "\n止痛药 - 34" +
                    "\n硬币 - 35";
                return true;
            }

            response = "<color=red>错误: 无效的指令</color>";
            return false;
        }
    }
}
