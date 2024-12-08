using DongShanAPI.Hint;
using DongShanAPI.Item.RomoveItem;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Linq;
using UnityEngine;

namespace XZPlugin
{
    class CoinLikee
    {
        string NothingHint = "<align=left><size=25><color=yellow>[硬币抽奖]</color><color=blue>什么也没有发生</color></size></align>";
        string TpPlayerRoom = "<align=left><size=25><color=yellow>[硬币抽奖]</color><color=blue>你被传送到了一个地方</color></size></align>";
        string RandomItem = $"<align=left><size=25><color=yellow>[硬币抽奖]</color><color=blue>你抽到了一个物品</color></size></align>";
        string NukeBoom = "<align=left><size=25><color=yellow>[硬币抽奖]</color><color=blue>核弹已爆炸，传送终止</color></size></align>";
        string TPtoPlayer = "<align=left><size=25><color=yellow>[硬币抽奖]</color><color=blue>你被传送到了一名人类身旁 他会是人类吗？</color></size></align>";
        string DieHint = "<align=left><size=25><color=yellow>[硬币抽奖]</color><color=blue>你抽奖抽死了</color></size></align>";
        string HealthHint = "<align=left><size=25><color=yellow>[硬币抽奖]</color><color=blue>你被治疗了</color></size></align>";
        string UpHealthHint = "<align=left><size=25><color=yellow>[硬币抽奖]</color><color=blue>你被的血量提升了! </color></size></align>";
        public void OnItemDropping(DroppingItemEventArgs ev)
        {
            if (ev.Item.id == ItemType.Coin)
            {
                ev.IsAllowed = false;

                RomoveItem.RemoveItem(ev.Player,ItemType.Coin);

                int lottery = UnityEngine.Random.Range(1, 101);

                if (lottery <= 5)
                {
                    KillPlayer(ev.Player);// 概率杀玩家
                }
                else if (lottery <= 40)
                {
                    GiveItem(ev.Player);// 概率给物品
                }
                else if (lottery <= 70)
                {
                    TpPlayertoRoomStart(ev.Player);// 概率传玩家到房间
                }
                else if (lottery <= 85)
                {
                    TptoPlayer(ev.Player);// 概率传玩家
                }
                else if (lottery <= 95)
                {
                    HealthPlayer(ev.Player);// 概率治疗
                }
                else if (lottery <= 99)
                {
                    UpHealthPlayer(ev.Player);// 概率提升血量
                }
                else
                {
                    Nothing(ev.Player);// 概率无事发生
                }

            }
        }

        private void Nothing(Player player)
        {
            player.RueiHint(400, NothingHint, 3);// 放放放放放放放放HINT
        }
        private void GiveItem(Player player)
        {
            var TrueItems = Enum.GetValues(typeof(ItemType))
                                  .Cast<ItemType>()
                                  .Where(item => (int)item >= 0 && (int)item <= 41)
                                  .ToList();

            ItemType randomItem = TrueItems[UnityEngine.Random.Range(0, TrueItems.Count)];

            player.AddItem(randomItem);

            player.RueiHint(400, RandomItem, 3);// 放放放放放放放放HINT
        }
        private void TpPlayertoRoom(Player player)
        {
            player.RueiHint(400, TpPlayerRoom, 3); // 放放放放放放放放HINT
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

        public void TpPlayertoRoomStart(Player player)
        {
            if (!Warhead.IsDetonated)
            {
                TpPlayertoRoom(player);
            }
            else
            {
                player.RueiHint(400, NukeBoom, 3);// 放放放放放放放放HINT
            }
        }
        private void TptoPlayer(Player player)
        {
            player.RueiHint(400, TPtoPlayer, 3);
            var ply = Player.List.Where(p => p != player).ToList();
            if (ply.Count > 0)
            {
                var target = ply[UnityEngine.Random.Range(0, ply.Count)];
                player.Position = target.Position;
            }
        }

        private void KillPlayer(Player player)
        {
            player.RueiHint(400, DieHint, 3); // 放放放放放放放放HINT
            player.Kill();
        }

        private void HealthPlayer(Player player)
        {
            player.RueiHint(400, HealthHint, 3);// 放放放放放放放放HINT
            player.Health = player.MaxHealth;
        }

        private void UpHealthPlayer(Player player)
        {
            player.RueiHint(400, UpHealthHint, 3);// 放放放放放放放放HINT
            player.MaxHealth = 350;
            player.Health = 350;
        }
    }
}
