using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using DongShanAPI.Item.Spawn;
using DongShanAPI.SCA;

namespace XZPlugin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class AdvancedItemSpawnCommand : ICommand
    {
        public string Command { get; } = "spi";
        public string[] Aliases { get; } = new string[] { };
        public string Description { get; } = "[ServerCommandAdmin] 在指定的玩家或所有玩家的位置生成指定体积的物品";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count < 2 || arguments.Count > 6)
            {
                response = "用法：.spi <玩家ID/all> <物品ID> [x] [y] [z] [数量]";
                return false;
            }

            if (!(sender is PlayerCommandSender playerSender))
            {
                response = "<color=red>错误：此命令只能由玩家使用</color>";
                return false;
            }

            Player player = Player.Get(playerSender.Nickname);
            if (player == null)
            {
                response = "<color=red>错误：找不到命令使用者</color>";
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

            string targetPlayerId = arguments.At(0);
            Player targetPlayer = null;
            bool targetAll = targetPlayerId.Equals("all", StringComparison.OrdinalIgnoreCase);

            if (!targetAll)
            {
                targetPlayer = Player.Get(targetPlayerId);
                if (targetPlayer == null)
                {
                    response = $"<color=red>错误：找不到玩家</color>";
                    return false;
                }
            }

            if (!Enum.TryParse(arguments.At(1), true, out ItemType itemType))
            {
                response = $"<color=red>无效的物品类型：{arguments.At(1)}</color>";
                return false;
            }

            float x = 1f, y = 1f, z = 1f;
            if (arguments.Count > 2 && !float.TryParse(arguments.At(2), out x))
            {
                response = "<color=red>无效的X尺寸</color>";
                return false;
            }

            if (arguments.Count > 3 && !float.TryParse(arguments.At(3), out y))
            {
                response = "<color=red>无效的Y尺寸</color>";
                return false;
            }

            if (arguments.Count > 4 && !float.TryParse(arguments.At(4), out z))
            {
                response = "<color=red>无效的Z尺寸</color>";
                return false;
            }

            int quantity = 1;
            if (arguments.Count == 6 && !int.TryParse(arguments.At(5), out quantity))
            {
                response = "<color=red>无效的数量</color>";
                return false;
            }

            if (targetAll)
            {
                foreach (var p in Player.List)
                {
                    if (p.Role != RoleType.Spectator && p.Role != RoleType.None)
                    {
                        for (int i = 0; i < quantity; i++)
                        {
                            p.SpawnItem(itemType, x, y, z);
                        }
                    }
                }
                response = $"<color=green>成功为所有玩家生成{quantity}个{itemType}</color>";
            }
            else
            {
                for (int i = 0; i < quantity; i++)
                {
                    targetPlayer.SpawnItem(itemType, x, y, z);
                }
                response = $"<color=green>成功为玩家{targetPlayer.Nickname}生成{quantity}个{itemType}</color>";
            }

            foreach (Player p in Player.List)
            {
                p.SendConsoleMessage($"{playerSender.Nickname}使用了生成物品命令！", "管理员操作日志");
            }

            return true;
        }
    }
}
