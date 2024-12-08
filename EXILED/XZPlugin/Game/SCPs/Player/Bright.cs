using DongShanAPI.Hint;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System;
using System.Linq;

namespace XZPlugin
{
    internal class Bright
    {
        private const int MaxReUse = 3;
        private int ReUse = 0;
        string m = "<b>你是\n<size=150>亮亮博士</size>\n拥有很强的装备, 亮不亮</b>";

        public void OnRespawning(RespawningTeamEventArgs ev)
        {
            if (ev.NextKnownTeam == Respawning.SpawnableTeamType.NineTailedFox)
            {
                ReStartBrightDr(ev);
            }
        }

        private void ReStartBrightDr(RespawningTeamEventArgs ev)
        {
            var 选择亮亮 = Player.List.FirstOrDefault(p => p.DisplayNickname.Contains(" | 亮亮 | "));

            if (选择亮亮 != null)
            {
                return;
            }

            var 随机玩家 = ev.Players.OrderBy(x => Guid.NewGuid()).FirstOrDefault();

            if (随机玩家 != null && !随机玩家.DisplayNickname.Contains(" | 亮亮 | "))
            {
                随机玩家.ClearInventory();
                Timing.CallDelayed(0.5f, () => SteartBrightDr(随机玩家));
            }
            else if (ReUse < MaxReUse)
            {
                ReUse++;
                Timing.CallDelayed(0.2f, () => ReStartBrightDr(ev));

            }
        }
        private void SteartBrightDr(Player player)
        {
            if (player.Inventory.items.Count == 0)
            {
                string NewNmae = $"{player.Id} | 亮亮 | {player.Nickname}";
                player.DisplayNickname = NewNmae;
                player.RueiHint(400, m, 10);
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
            }
            else
            {
                player.ClearInventory();
                Timing.CallDelayed(0.2f, () => SteartBrightDr(player));
            }
        }

        public void OnPlayerDied(DiedEventArgs ev)
        {
            string NewNmae = $"{ev.Target.Id} | {ev.Target.Nickname}";
            ev.Target.DisplayNickname = NewNmae;
        }
    }
}