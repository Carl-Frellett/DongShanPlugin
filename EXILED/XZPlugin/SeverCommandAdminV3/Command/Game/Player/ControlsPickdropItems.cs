using CommandSystem;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using DongShanAPI.SCA;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ControlsPickdropItemsCommand : ICommand
    {
        public string Command { get; } = "itmd";
        public string[] Aliases { get; } = new string[] { };
        public string Description { get; } = "[ServerCommandAdmin] 控制玩家的拾取与丢弃物品";

        private static Dictionary<Player, (bool, bool)> itemModes = new Dictionary<Player, (bool, bool)>();

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

            if (arguments.Count < 2)
            {
                response = "<color=red>错误：参数不足</color>";
                return false;
            }

            string option = arguments.At(0).ToLower();
            string target = arguments.At(1);

            bool canPickup, canDrop;
            switch (option)
            {
                case "z":
                    canPickup = false;
                    canDrop = true;
                    break;
                case "y":
                    canPickup = true;
                    canDrop = false;
                    break;
                case "c":
                    canPickup = false;
                    canDrop = false;
                    break;
                case "x":
                    canPickup = true;
                    canDrop = true;
                    break;
                default:
                    response = "<color=red>错误：无效的模式参数\n参数列表: \n z - 不允许拾取，允许丢弃\n y - 允许拾取，不允许丢弃\n c - 都不允许\n x - 都允许</color>";
                    return false;
            }

            if (target.ToLower() == "all")
            {
                foreach (Player p in Player.List)
                {
                    SetItemMode(p, canPickup, canDrop);
                }
                response = $"<color=green>已设置所有玩家{(canPickup ? "可以" : "无法")}拾取物品，{(canDrop ? "可以" : "无法")}丢弃物品</color>";
                return true;
            }

            if (!uint.TryParse(target, out uint playerId))
            {
                response = "<color=red>错误：无效的玩家ID</color>";
                return false;
            }

            Player oPlayer = Player.Get((int)playerId);
            if (oPlayer == null)
            {
                response = $"<color=red>错误：无法找到ID为 {playerId} 的玩家</color>";
                return false;
            }

            SetItemMode(oPlayer, canPickup, canDrop);
            response = $"<color=green>已设置 {oPlayer.Nickname} {(canPickup ? "可以" : "无法")}拾取物品，{(canDrop ? "可以" : "无法")}丢弃物品</color>";
            foreach (Player p in Player.List)
            {
                p.SendConsoleMessage($"{playerSender.Nickname}控制了玩家物品掉落与拾取指令!", "管理员操作日志");
            }
            return true;
        }

        private void SetItemMode(Player player, bool canPickup, bool canDrop)
        {
            if (!itemModes.ContainsKey(player))
            {
                itemModes[player] = (true, true);
                Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
                Exiled.Events.Handlers.Player.DroppingItem += OnDroppingItem;
            }

            itemModes[player] = (canPickup, canDrop);
        }

        private void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (itemModes.TryGetValue(ev.Player, out var mode) && !mode.Item1)
            {
                ev.IsAllowed = false;
            }
        }

        private void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (itemModes.TryGetValue(ev.Player, out var mode) && !mode.Item2)
            {
                ev.IsAllowed = false;
            }
        }
    }
}
