using CommandSystem;
using Exiled.API.Features;
using Mirror;
using RemoteAdmin;
using System;
using DongShanAPI.SCA;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ClearCommand : ICommand
    {
        public string Command { get; } = "cl";
        public string[] Aliases { get; } = new string[] { };
        public string Description { get; } = "[ServerCommandAdmin] 清理场地上的物品或尸体";

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

            if (arguments.Count == 0)
            {
                response = "<color=red>错误：请提供参数 'st' (尸体) 或 'wp' (物品)</color>";
                return false;
            }
            foreach (Player p in Player.List)
            {
                p.SendConsoleMessage($"{playerSender.Nickname}使用了垃圾清理指令!", "管理员操作日志");
            }

            var option = arguments.At(0).ToLower();
            switch (option)
            {
                case "st":
                    CleanupCorpses();
                    response = "<color=green>所有尸体已被清除</color>";
                    return true;
                case "wp":
                    CleanupItems();
                    response = "<color=green>所有物品已被清除</color>";
                    return true;
                default:
                    response = "<color=red>错误：无效的参数, 请输入 'st' 或 'wp'</color>";
                    return false;
            }

        }

        private void CleanupCorpses()
        {
            foreach (var ragdoll in UnityEngine.Object.FindObjectsOfType<Ragdoll>())
            {
                NetworkServer.Destroy(ragdoll.gameObject);
            }
            Map.Broadcast(4, "<size=40><color=yellow>[饿了喵-AD]</color></size>\n<size=35><color=blue>管理员清理了尸体</color></size>", Broadcast.BroadcastFlags.Normal);
        }

        private void CleanupItems()
        {
            foreach (var item in UnityEngine.Object.FindObjectsOfType<Pickup>())
            {
                NetworkServer.Destroy(item.gameObject);
            }
            Map.Broadcast(4, "<size=40><color=yellow>[饿了喵-AD]</color></size>\n<size=35><color=blue>管理员清理了物品</color></size>", Broadcast.BroadcastFlags.Normal);
        }
    }
}
