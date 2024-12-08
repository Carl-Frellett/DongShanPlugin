using CommandSystem;
using DongShanAPI.Hint;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Player = Exiled.API.Features.Player;
using PlayerEvent = Exiled.Events.Handlers.Player;
using DongShanAPI.SCA;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Start550Command : ICommand
    {
        public string Command { get; } = "start550";
        public string[] Aliases { get; } = new string[] { };
        public string Description { get; } = "[ServerCommandAdmin] 指定某一名玩家成为550";

        public Player SCP550;

        string StartHint = "<b>你是\n<size=150><color=red>SCP-550</color></size>\n与SCP联手占领设施</b>";
        string DieHint = "<align=left><color=yellow><size=25>[收容通知]<color=red> SCP-550现已被收容</color></size></align>";

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
                response = "<color=yellow>指令格式: .start550 <玩家ID></color>";
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
                p.SendConsoleMessage($"{playerSender.Nickname}将{target.Nickname}变成SCP550!", "管理员操作日志");
            }
            SCP550 = target;
            Start550(target);

            response = $"<color=green>成功,{target.Nickname}已成为SCP550</color>";
            return true;
        }
        public void Start550(Player player)
        {
            var _49room = Map.Rooms.FirstOrDefault(room => room.Type == RoomType.Hcz049);

            player.Position = _49room.Position + new Vector3(0, 1, 0);

            player.SetRole(RoleType.Tutorial, true);

            string NewName = $"{player.Id} | SCP550 | {player.Nickname}";
            player.DisplayNickname = NewName;

            player.MaxHealth = 500;
            player.Health = 500;

            player.AddItem(ItemType.GunE11SR);
            player.AddItem(ItemType.GunUSP);
            player.AddItem(ItemType.Medkit);
            player.AddItem(ItemType.Medkit);
            player.AddItem(ItemType.KeycardChaosInsurgency);

            Scp096.TurnedPlayers.Add(player);
            Scp173.TurnedPlayers.Add(player);
            Timing.RunCoroutine(Health(player));


            player.RueiHint(400, StartHint, 10);

            PlayerEvent.Hurting += OnPlayerHurting;
            PlayerEvent.Died += OnDied;
            PlayerEvent.ChangingRole += OnChangRole;
        }
        private IEnumerator<float> Health(Player player)
        {
            while (player != null && player.Role == RoleType.Tutorial)
            {
                if (player.Health < player.MaxHealth)
                {
                    player.Health += 3;
                }
                yield return Timing.WaitForSeconds(1);
            }
        }

        public void OnPlayerHurting(HurtingEventArgs ev)
        {
            if (ev.Target == SCP550)
            {
                ev.Amount *= 0.2f;
            }

            if ((ev.Target == SCP550 || ev.Attacker == SCP550) && (ev.Attacker.Team == Team.SCP || ev.Target.Team == Team.SCP))
            {
                ev.IsAllowed = false;
            }
        }
        public void OnDied(DiedEventArgs ev)
        {
            if (ev.Target != null && ev.Target == SCP550)
            {
                Dieplayer(SCP550);
            }
        }
        public void OnChangRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player != null && ev.Player == SCP550)
            {
                Dieplayer(SCP550);
            }
        }

        public void Dieplayer(Player player)
        {
            string NewName = $"{player.Id} | {player.Nickname}";
            player.DisplayNickname = NewName;
            Scp096.TurnedPlayers.Clear();
            Scp173.TurnedPlayers.Clear();
            player.ARueiHint(400, DieHint, 4);

            PlayerEvent.Hurting -= OnPlayerHurting;
            PlayerEvent.Died -= OnDied;
            PlayerEvent.ChangingRole -= OnChangRole;

            player = null;
        }
    }
}
