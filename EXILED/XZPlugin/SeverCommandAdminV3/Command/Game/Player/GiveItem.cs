using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using DongShanAPI.SCA;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class GiveItemCommand : ICommand
    {
        public string Command { get; } = "GiveItem";
        public string[] Aliases { get; } = { "gv" };
        public string Description { get; } = "[ServerCommandAdmin] 给予玩家物品";

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
                response = "<color=red>错误：找不到命令使用者</color>";
                return false;
            }

            if (!AdminFileManager.IsAdmin(player.Nickname, player.IPAddress))
            {
                response = "<color=red>错误：你没有管理员权限</color>";
                return false;
            }

            if (!player.ReferenceHub.serverRoles.RemoteAdmin)
            {
                response = "<color=red>错误：未经过管理员验证!</color>";
                return false;
            }

            if (arguments.Count < 2)
            {
                response = "<color=yellow>指令格式: .gv [玩家ID/all] <物品ID></color>";
                return false;
            }

            string targetId = arguments.At(0);
            int itemId;

            if (!int.TryParse(arguments.At(1), out itemId))
            {
                response = "<color=red>错误：无效的物品ID</color>";
                return false;
            }

            ItemType item = (ItemType)itemId;

            if (!Enum.IsDefined(typeof(ItemType), item))
            {
                response = $"<color=red>无效的物品ID: {itemId}</color>";
                return false;
            }

            if (targetId.ToLower() == "all")
            {
                foreach (Player p in Player.List)
                {
                    p.AddItem(item);
                }

                response = $"<color=green>所有玩家已获得：{item}</color>";
                return true;
            }

            if (!int.TryParse(targetId, out int playerId))
            {
                response = "<color=red>错误：玩家ID必须为整数或'all'</color>";
                return false;
            }

            Player target = Player.Get(playerId);

            if (target == null)
            {
                response = $"<color=red>错误：未找到ID为{playerId}的玩家</color>";
                return false;
            }
            foreach (Player p in Player.List)
            {
                p.SendConsoleMessage($"{playerSender.Nickname}使用了物品给予指令!", "管理员操作日志");
            }
            target.AddItem(item);
            response = $"\n<color=green>玩家：{target.Nickname}\n已获得物品:{item}</color>";

            return true;
        }
    }
}
