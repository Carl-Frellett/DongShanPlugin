using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System.Linq;
using UnityEngine;
using DongShanAPI.Hint;
using Random = System.Random;
using DongShanAPI.Item.Spawn;

namespace XZPlugin
{
    class Kongtou
    {
        private readonly Random random = new Random();

        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            if (Warhead.IsDetonated)
            {
                return;
            }

            if (IsSpawKongtou())
            {
                Spawnkongtou();
            }
        }

        private bool IsSpawKongtou()
        {
            bool SCP = Player.List.Any(player => player.IsScp);
            bool ClassD = Player.List.Any(player => player.Role == RoleType.ClassD);

            int value;

            if (SCP && ClassD)
            {
                value = 60;
            }
            else if (SCP)
            {
                value = 30;
            }
            else
            {
                value = 10;
            }

            int roll = random.Next(100);
            return roll < value;
        }
        private void Spawnkongtou()
        {
            string kt = "<align=left><size=25><color=yellow>[天降空投]</color><color=blue>空投已在A, B门刷新! 请查收! </color></size></align>";
            foreach (Player p in Player.List)
            {
                p.RueiHint(400, kt, 3);
            }

            Room A = Map.Rooms.FirstOrDefault(room => room.Type == RoomType.EzGateA);
            Room B = Map.Rooms.FirstOrDefault(room => room.Type == RoomType.EzGateB);

            if (A != null)
            {
                Vector3 APos = A.Position;
                SpawnItems.SpawnPosItem(ItemType.GunUSP, APos);
                SpawnItems.SpawnPosItem(ItemType.GunE11SR, APos);
                SpawnItems.SpawnPosItem(ItemType.GunProject90, APos);
                SpawnItems.SpawnPosItem(ItemType.WeaponManagerTablet, APos);
                SpawnItems.SpawnPosItem(ItemType.Adrenaline, APos, 3);
                SpawnItems.SpawnPosItem(ItemType.Coin, APos);
                SpawnItems.SpawnPosItem(ItemType.Medkit, APos, 5);
                
            }

            if (B != null)
            {
                Vector3 BPos = B.Position;
                SpawnItems.SpawnPosItem(ItemType.GunUSP, BPos);
                SpawnItems.SpawnPosItem(ItemType.GunE11SR, BPos);
                SpawnItems.SpawnPosItem(ItemType.GunProject90, BPos);
                SpawnItems.SpawnPosItem(ItemType.Adrenaline, BPos, 3);
                SpawnItems.SpawnPosItem(ItemType.Coin, BPos);
                SpawnItems.SpawnPosItem(ItemType.Medkit, BPos, 5);
            }
        }
    }
}
