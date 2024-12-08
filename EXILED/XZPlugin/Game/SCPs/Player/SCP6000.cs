using DongShanAPI.Hint;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace XZPlugin
{
    class S6000
    {
        public Player SCP6000;
        public readonly List<CoroutineHandle> coroutine = new List<CoroutineHandle>();
        public readonly System.Random random = new System.Random();
        string DieHint = "<align=left><color=yellow><size=25>[收容通知]<color=red> SCP-6000现已被收容</color></size></align>";
        string StartHint = "<b>你是\n<size=150><color=red>SCP-6000</color></size>\n丢弃手电筒以传送</b>";
        public void OnRoundStart()
        {
            Timing.CallDelayed(2f, () =>
            {
                StartSCP6000();
            });
        }
        public void StartSCP6000()
        {
            var ClassD = Player.List
                .Where(p => p.Role == RoleType.ClassD)
                .Where(p => !p.DisplayNickname.Contains("SCP181") && !p.DisplayNickname.Contains("SCP2498")).ToList();

            if (ClassD.Count > 3)
            {
                int index = new System.Random().Next(ClassD.Count);
                SCP6000 = ClassD[index];

                string NewName = $"{SCP6000.Id} | SCP6000 | {SCP6000.Nickname}";
                SCP6000.DisplayNickname = NewName;

                SCP6000.RueiHint(400, StartHint, 10);
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
            }
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
                yield return Timing.WaitForSeconds(60);

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
