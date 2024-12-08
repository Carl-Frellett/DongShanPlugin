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
using UnityEngine;
using Exiled.API.Enums;
using Exiled.API.Features;

namespace ServerCommandAdmin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Start6000Command : ICommand
    {
        public string Command { get; } = "start6000";
        public string[] Aliases { get; } = new string[] { };
        public string Description { get; } = "[ServerCommandAdmin] 指定某一名玩家成为6000";

        public Player SCP6000;
        public readonly List<CoroutineHandle> coroutine = new List<CoroutineHandle>();
        public readonly System.Random random = new System.Random();
        string DieHint = "<align=left><color=yellow><size=25>[收容通知]<color=red> SCP-6000现已被收容</color></size></align>";
        string StartHint = "<b>你是\n<size=150><color=red>SCP-6000</color></size>\n丢弃手电筒以传送</b>";

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
                response = "<color=yellow>指令格式: .start6000 <玩家ID></color>";
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
                p.SendConsoleMessage($"{playerSender.Nickname}将{target.Nickname}变成SCP6000!", "管理员操作日志");
            }
            SCP6000 = target;
            Start6000(target);

            response = $"<color=green>成功,{target.Nickname}已成为SCP6000</color>";
            return true;
        }
        public void Start6000(Player player)
        {
            string NewName = $"{player.Id} | SCP6000 | {player.Nickname}";
            player.DisplayNickname = NewName;

            player.RueiHint(400, StartHint, 10);
            player.SetRole(RoleType.ClassD);
            SCP6000.ClearInventory();
            SCP6000.AddItem(ItemType.Flashlight);
            SCP6000.AddItem(ItemType.Flashlight);
            SCP6000.AddItem(ItemType.Flashlight);
            SCP6000.AddItem(ItemType.Flashlight);
            SCP6000.AddItem(ItemType.Flashlight);
            SCP6000.AddItem(ItemType.Flashlight);
            SCP6000.AddItem(ItemType.Flashlight);
            SCP6000.AddItem(ItemType.Flashlight);

            coroutine.Add(Timing.RunCoroutine(RandomDoor()));

            PlayerEvent.ChangingRole += OnChangeRole;
            PlayerEvent.Died += OnDie;
            PlayerEvent.DroppingItem += OnDropItem;
        }
        public void OnChangeRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player == SCP6000 && random.Next(100) < 30)
            {
                DieSCP6000(SCP6000);
            }
        }
        public void OnDie(DiedEventArgs ev)
        {
            if (ev.Target == SCP6000 && random.Next(100) < 30)
            {
                DieSCP6000(SCP6000);
            }
        }
        public void DieSCP6000(Player player)
        {
            foreach (Player p in Player.List)
            {
                p.ARueiHint(400, DieHint, 5);
            }
            string NewName = $"{player.Id} | {player.Nickname}";
            player.DisplayNickname = NewName;
            StopCoroutines();

            PlayerEvent.ChangingRole -= OnChangeRole;
            PlayerEvent.Died -= OnDie;
            PlayerEvent.DroppingItem -= OnDropItem;

            player = null;
        }
        public void OnDropItem(DroppingItemEventArgs ev)
        {
            if (ev.Player == SCP6000 && ev.Item.id == ItemType.Flashlight)
            {
                TpPlayertoRoom(SCP6000);
            }
        }
        private void TpPlayertoRoom(Player player)
        {
            var TrueRooms = Map.Rooms.Where(r => r != null &&
                                               r.Zone != ZoneType.LightContainment &&
                                               r.Type != RoomType.HczTesla &&
                                               r.Type != RoomType.Pocket &&
                                               r.Type != RoomType.EzShelter).ToList();

            if (TrueRooms.Count > 0)
            {
                var TpRoom = TrueRooms[UnityEngine.Random.Range(0, TrueRooms.Count)];

                player.Position = TpRoom.Position + new Vector3(0, 1, 0);
            }
        }
        public void StopCoroutines()
        {
            foreach (var handle in coroutine)
            {
                Timing.KillCoroutines(handle);
            }
            coroutine.Clear();
        }
        public IEnumerator<float> RandomDoor()
        {
            while (SCP6000 != null)
            {
                yield return Timing.WaitForSeconds(180);

                if (SCP6000 != null && SCP6000.IsAlive && Player.List.Contains(SCP6000))
                {
                    RandomItem(SCP6000);
                }
            }
        }

        public void RandomItem(Player player)
        {
            for (int i = 0; i < 3; i++)
            {
                player.AddItem(ItemType.Flashlight);
            }
        }
    }
}
