using CommandSystem;
using System;
using System.Collections.Generic;

namespace XZPlugin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class HelpCommand : ICommand
    {
        public string Command { get; } = "help";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "指令帮助列表";

        private static readonly Dictionary<string, string> SCACommandsDescription = new Dictionary<string, string>
        {
            {"help sca", "获取SCA的指令列表"},
            {"<size=0.01>.</size>cl", "清理物品与尸体 - 用法: \n.cl st/wp  (尸体/物品)"},
            {"nuk", "控制核弹 - 用法: \n.nuk st/sp/de  (启动/关闭/引爆)"},
            {"cl", "清理物品与尸体 - 用法: \n.cl st/wp  (尸体/物品)"},
            {"by", "获取最高门禁权限 - 用法: \n.by <玩家ID/all>"},
            {"fcp", "改变玩家的角色 - 用法: \n.fcp <玩家ID/all> [角色ID] [是否保留物品与位置 y/n]"},
            {"spi", "生成可控制大小与数量的物品 - 用法: \n.spi <玩家ID/all> <物品ID> [长] [宽] [高] [数量]"},
            {"cin", "清空玩家背包 - 用法: \n.cin <玩家ID/all>"},
            {"itmd", "控制玩家的拾取与丢弃物品 - 用法: \n.itmd <玩家ID/all> [参数 z/y/c/x] \n参数列表: \n z - 不允许拾取，允许丢弃\n y - 允许拾取，不允许丢弃\n c - 都不允许\n x - 都允许"},
            {"gv", "给予玩家物品 - 用法: \n.gv <玩家ID/all> [物品ID]"},
            {"gd", "无敌模式 - 用法: \n.gd <玩家ID/all>"},
            {"hp", "修改玩家的生命值 - 用法: \n.hp <是否为临时血量 y/z> <玩家ID或all> <数值>"},
            {"kill", "杀死玩家 - 用法: \n.kill <玩家ID/all>"},
            {"Noclip", "飞行 - 用法: \n.np <玩家ID/all>"},
            {"pmr", "修改玩家的模型 - 用法: \n.pmr <玩家ID/all> [角色ID]"},
            {"size", "修改玩家的体积 - 用法: \n.size <玩家ID/all> [长] [宽] [高]"},
            {"tp", "将一名玩家传送到另一名玩家的位置 - 用法: \n.tp <玩家ID/all> <玩家ID>"},
            {"rp", "强制刷新阵营 - 用法: \n.rp <ntf/ci>"},
            {"llr", "锁等待大厅 - 用法: \n.llr"},
            {"lr", "锁回合 - 用法: \n.lr"},
            {"fsr", "重启回合 - 用法: \n.fsr"},
            {"frr", "开始回合 - 用法: \n.frr"},
            {"ban", "封禁玩家 - 用法: \n.ban <玩家ID> <时间> <原因>"},
            {"kick", "踢出玩家 - 用法: \n.kick <玩家ID> <原因>"},
            {"ow", "监管模式 - 用法: \n.ow <玩家ID/all>"},
            {"start181", "将某一名玩家设置为181 - 用法: \n.start181 <玩家ID>"},
            {"start2498", "将某一名玩家设置为2498 - 用法: \n.start2498 <玩家ID>"},
            {"start550", "将某一名玩家设置为550 - 用法: \n.start550 <玩家ID>"},
            {"start703", "将某一名玩家设置为703 - 用法: \n.start703 <玩家ID>"},
            {"start6000", "将某一名玩家设置为6000 - 用法: \n.start6000 <玩家ID>"},
            {"startbb", "将某一名玩家设置为亮亮博士 - 用法: \n.startbb <玩家ID>"},
         };
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 0)
            {
                response =
                    "\n==========指令列表==========\n" +
                    "help - 获取指令列表\n" +
                    "kill - 自杀\n" +
                    "bc - 全体聊天\n" +
                    "c - 团队聊天\n" +
                    "q - 瞧瞧话\n" +
                    "ac - 管理求助\n" +
                    "pi - 修复自己的名称\n" +
                    "p - 显示服务器信息\n" +
                    "tps - 显示服务器TPS\n" +
                    "TX - DD投降┗( T﹏T )┛\n" +
                    "usp - 显示你的USP当前可以造成多少伤害\n" +
                    "ItemList - 显示物品列表\n" +
                    "RoleList - 显示角色列表\n" +
                    "============================";
                return true;
            }

            if (arguments.Count >= 2 && arguments.At(0).ToLower() == "sca")
            {
                var commandKey = arguments.At(1).ToLower();

                if (SCACommandsDescription.TryGetValue(commandKey, out var commandDesc))
                {
                    response = $"\n{commandKey}: {commandDesc}";
                    return true;
                }
                else
                {
                    response = $"<red>Error: '{commandKey}' 没有找到相关信息.</red>";
                    return false; 
                }
            }

            if (arguments.Count == 1 && arguments.At(0).ToLower() == "sca")
            {
                response = "\n==========SCA指令列表==========\n" +
                "help sca - 获取SCA的指令列表\n" +
                "cl - 清理物品与尸体\n" +
                "nuk - 控制核弹\n" +
                "cl - 清理物品与尸体\n" +
                "by - 获取最高门禁权限\n" +
                "fcp - 改变玩家的角色\n" +
                "spi - 生成可控制大小与数量的物品\n" +
                "cin - 清空玩家背包\n" +
                "itmd - 控制玩家的拾取与丢弃物品\n" +
                "gv - 给予玩家物品\n" +
                "gd - 无敌模式\n" +
                "hp - 修改玩家的生命值\n" +
                "kill - 杀死玩家\n" +
                "Noclip - 飞行\n" +
                "pmr - 修改玩家的模型\n" +
                "size - 修改玩家的体积\n" +
                "tp - 将一名玩家传送到另一名玩家的位置\n" +
                "rp - 强制刷新阵营\n" +
                "llr - 锁等待大厅\n" +
                "lr - 锁回合\n" +
                "fsr - 重启回合\n" +
                "frr - 开始回合\n" +
                "ban - 封禁玩家\n" +
                "kick - 踢出玩家\n" +
                "ow - 监管模式\n" +
                "start181 - 将某一名玩家设置为181\n" +
                "start2498 - 将某一名玩家设置为2498\n" +
                "start550 - 将某一名玩家设置为550\n" +
                "start703 - 将某一名玩家设置为703\n" +
                "start6000 - 将某一名玩家设置为6000\n" +
                "startbb - 将某一名玩家设置为亮亮博士\n" +
                "=============================";
                return true;
            }

            response = "<color=red>错误: 无效的指令</color>";
            return false;
        }
    }
}
