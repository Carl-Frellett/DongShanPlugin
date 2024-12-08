using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using DongShanAPI.SCA;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class HealthCommand : ICommand
    {
        public string Command { get; } = "hp";
        public string[] Aliases { get; } = new string[] { };
        public string Description { get; } = "[ServerCommandAdmin] 修改玩家的血量(临时- y 血量上限- z)";

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

            if (arguments.Count != 3)
            {
                response = "<color=red>错误：参数数量不正确。用法: .hp <y/z> <玩家ID或all> <数值></color>";
                return false;
            }

            string option = arguments.At(0);
            string target = arguments.At(1);
            if (!int.TryParse(arguments.At(2), out int hp))
            {
                response = "<color=red>错误：无效的数值参数</color>";
                return false;
            }

            if (option.ToLower() == "y")
            {
                if (target.ToLower() == "all")
                {
                    foreach (Player p in Player.List)
                    {
                        SetPlayerHealth(p, hp);
                    }
                    response = "<color=green>成功：已修改所有玩家的临时血量</color>";
                }
                else
                {
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
                    SetPlayerHealth(oPlayer, hp);
                    response = $"<color=green>成功：已修改玩家 {oPlayer.Nickname} 的临时血量</color>";
                }
            }
            else if (option.ToLower() == "z")
            {
                if (target.ToLower() == "all")
                {
                    foreach (Player p in Player.List)
                    {
                        SetPlayerMaxHealth(p, hp);
                    }
                    response = "<color=green>成功：已修改所有玩家的血量上限</color>";
                }
                else
                {
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
                    SetPlayerMaxHealth(oPlayer, hp);
                    response = $"<color=green>成功：已修改玩家 {oPlayer.Nickname} 的血量上限</color>";
                }
            }
            else
            {
                response = "<color=red>错误：无效的类型参数。用法: .hp <y/z> <玩家ID或all> <数值></color>";
                return false;
            }
            foreach (Player p in Player.List)
            {
                p.SendConsoleMessage($"{playerSender.Nickname}使用了血量修改指令!", "管理员操作日志");
            }
            return true;
        }

        private void SetPlayerHealth(Player player, int hp)
        {
            player.Health = hp;
        }

        private void SetPlayerMaxHealth(Player player, int hp)
        {
            player.MaxHealth = hp;
            player.Health = hp;
        }
    }
}