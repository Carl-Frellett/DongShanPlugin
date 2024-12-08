using CommandSystem;
using DongShanAPI.Hint;
using Exiled.Events.EventArgs;
using RemoteAdmin;
using System;
using Player = Exiled.API.Features.Player;
using DongShanAPI.SCA;
using PlayerEvent = Exiled.Events.Handlers.Player;
using MEC;
using System.Collections.Generic;
using System.Linq;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Start703Command : ICommand
    {
        public string Command { get; } = "start703";
        public string[] Aliases { get; } = new string[] { };
        public string Description { get; } = "[ServerCommandAdmin] 指定某一名玩家成为703";

        string DieHint = "<align=left><color=yellow><size=25>[收容通知]</color><color=red>SCP-703现已被收容</color></size></align>";
        string PlayerHint = "<size=25>[个人消息] 703将在3秒后刷新物品! ";
        string StartHint = "<b>你是\n<size=100><color=red>SCP-703</color></size>\n每过3分钟刷东西, 有基础物品</b>";

        public Player SCP703;
        public readonly List<CoroutineHandle> coroutine = new List<CoroutineHandle>();
        public readonly Random random = new Random();

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
                response = "<color=yellow>指令格式: .start703 <玩家ID></color>";
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
                p.SendConsoleMessage($"{playerSender.Nickname}将{target.Nickname}变成SCP703!", "管理员操作日志");
            }
            SCP703 = target;
            Start703(target);

            response = $"<color=green>成功,{target.Nickname}已成为SCP703</color>";
            return true;
        }
        public void Start703(Player player)
        {
            string NewNmae = $"{player.Id} | SCP703 | {player.Nickname}";
            player.DisplayNickname = NewNmae;
            player.RueiHint(400, StartHint, 10);
            Timing.CallDelayed(5f, () => coroutine.Add(Timing.RunCoroutine(ItemGivingRoutine(player))));

            PlayerEvent.Died += OnPlayerDied;
            PlayerEvent.ChangingRole += OnChangingRole;
        }
        public void OnPlayerDied(DiedEventArgs ev)
        {
            if (ev.Target != null && ev.Target == SCP703)
            {
                DieSCP703(SCP703);
            }
        }

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player != null && ev.Player == SCP703)
            {
                DieSCP703(SCP703);
            }
        }
        public void DieSCP703(Player player)
        {
            string NewName = $"{player.Id} | {player.Nickname}";
            player.DisplayNickname = NewName;
            player = null;
            StopCoroutines();

            foreach (Player p in Player.List)
            {
                p.ARueiHint(400, DieHint, 5);
            }

            PlayerEvent.Died -= OnPlayerDied;
            PlayerEvent.ChangingRole -= OnChangingRole;
        }
        public void StopCoroutines()
        {
            foreach (var handle in coroutine)
            {
                Timing.KillCoroutines(handle);
            }
            coroutine.Clear();
        }
        public IEnumerator<float> ItemGivingRoutine(Player player)
        {
            while (player != null)
            {
                yield return Timing.WaitForSeconds(180 - 3);

                player.RueiHint(400, PlayerHint, 3);

                yield return Timing.WaitForSeconds(3);

                if (SCP703 != null && player.IsAlive && Player.List.Contains(player))
                {
                    RandomItem(player);
                }
            }
        }

        public void RandomItem(Player player)
        {
            for (int i = 0; i < 3; i++)
            {
                player.AddItem((ItemType)random.Next(0, 35));
            }
        }
    }
}
