using CommandSystem;
using DongShanAPI.Hint;
using Exiled.Events.EventArgs;
using RemoteAdmin;
using System;
using Player = Exiled.API.Features.Player;
using DongShanAPI.SCA;
using PlayerEvent = Exiled.Events.Handlers.Player;
using MEC;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class StartBrightCommand : ICommand
    {
        public string Command { get; } = "startbb";
        public string[] Aliases { get; } = new string[] { };
        public string Description { get; } = "[ServerCommandAdmin] 指定某一名玩家成为亮亮";

        public Player Bright;
        string m = "<b>你是\n<size=150>亮亮博士</size>\n拥有很强的装备, 亮不亮</b>";

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
                response = "<color=yellow>指令格式: .start2498 <玩家ID></color>";
                return false;
            }

            if (!int.TryParse(arguments.At(0), out int targetId))
            {
                response = "<color=red>错误：玩家ID必须为整数</color>";
                return false;
            }

            Player target = Player.Get(targetId);
            if (target == null)
            {
                response = $"<color=red>错误：未找到ID为{targetId}的玩家</color>";
                return false;
            }
            foreach (Player p in Player.List)
            {
                p.SendConsoleMessage($"{playerSender.Nickname}将{target.Nickname}变成Bright!", "管理员操作日志");
            }
            Bright = target;
            SteartBrightDr(target);

            response = $"<color=green>成功,{target.Nickname}已成为Bright</color>";
            return true;
        }
        private void SteartBrightDr(Player player)
        {
            string NewNmae = $"{player.Id} | 亮亮 | {player.Nickname}";
            player.DisplayNickname = NewNmae;
            player.RueiHint(400, m, 10);
            player.SetRole(RoleType.NtfLieutenant);
            player.SetRole(RoleType.Scientist, true);
            player.ClearInventory();
            player.MaxHealth = 300;
            player.Health = 300;
            player.AddItem(ItemType.GunE11SR);
            player.AddItem(ItemType.GunUSP);
            player.AddItem(ItemType.KeycardNTFLieutenant);
            player.AddItem(ItemType.MicroHID);
            player.AddItem(ItemType.Medkit);
            player.AddItem(ItemType.SCP207);
            player.AddItem(ItemType.SCP207);

            PlayerEvent.Died += OnPlayerDied;
        }

        public void OnPlayerDied(DiedEventArgs ev)
        {
            string NewNmae = $"{ev.Target.Id} | {ev.Target.Nickname}";
            ev.Target.DisplayNickname = NewNmae;

            PlayerEvent.Died -= OnPlayerDied;
        }
    }
}
