using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using Respawning;
using System;
using DongShanAPI.SCA;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class RespawnCommand : ICommand
    {
        public string Command { get; } = "respawn";
        public string[] Aliases { get; } = new string[] { "rp" };
        public string Description { get; } = "[ServerCommandAdmin] 刷新阵营";

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

            if (arguments.Count != 1)
            {
                response = "<color=red>用法: .rp <NTF/CI></color>";
                return false;
            }

            string faction = arguments.At(0).ToUpper();
            if (faction != "NTF" && faction != "CI")
            {
                response = "<color=red>错误：无效的阵营名称</color>";
                return false;
            }

            if (faction == "NTF")
            {
                RespawnEffectsController.PlayCassieAnnouncement("Done", false, false);
                Map.Broadcast(3, "<size=20><color=blue>管理员强制刷新了阵营</color></size>");
                RespawnManager.Singleton.ForceSpawnTeam(SpawnableTeamType.NineTailedFox);
            }
            else if (faction == "CI")
            {
                RespawnEffectsController.PlayCassieAnnouncement("Done", false, false);
                RespawnManager.Singleton.ForceSpawnTeam(SpawnableTeamType.ChaosInsurgency);
                Map.Broadcast(3, "<size=20><color=green>管理员强制刷新了阵营</color></size>");
            }
            foreach (Player p in Player.List)
            {
                p.SendConsoleMessage($"{playerSender.Nickname}使用了强制刷新阵营指令!", "管理员操作日志");
            }
            response = $"<color=green>成功刷新</color>";
            return true;
        }
    }
}
