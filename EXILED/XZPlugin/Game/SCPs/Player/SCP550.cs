using DongShanAPI.Hint;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;
using PlayerEvent = Exiled.Events.Handlers.Player;

namespace XZPlugin
{
    public class S550
    {
        private const int MaxReUse = 2;
        private int ReUse = 0;

        public Player SCP550;

        string StartHint = "<b>你是\n<size=150><color=red>SCP-550</color></size>\n与SCP联手占领设施</b>";
        string DieHint = "<align=left><color=yellow><size=25>[收容通知]<color=red> SCP-550现已被收容</color></size></align>";

        public void OnRespawning(RespawningTeamEventArgs ev)
        {
            if (ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency)
            {
                ReStart550(ev);
            }
        }

        private void ReStart550(RespawningTeamEventArgs ev)
        {
            var 选择550 = Player.List.FirstOrDefault(p => p.DisplayNickname.Contains(" | SCP550 | "));

            if (选择550 != null)
            {
                return;
            }

            var 随机玩家 = ev.Players.OrderBy(x => Guid.NewGuid()).FirstOrDefault();

            if (随机玩家 != null && !随机玩家.DisplayNickname.Contains(" | SCP550 | "))
            {
                随机玩家.ClearInventory();
                Timing.CallDelayed(0.5f, () => Start550(随机玩家));
            }
            else if (ReUse < MaxReUse)
            {
                ReUse++;
                Timing.CallDelayed(0.2f, () => ReStart550(ev));

            }
        }
        public void Start550(Player player)
        {
            SCP550 = player;

            var _49room = Map.Rooms.FirstOrDefault(room => room.Type == RoomType.Hcz049);

            player.Position = _49room.Position + new Vector3(0, 1, 0);

            player.SetRole(RoleType.Tutorial, true);

            string NewName = $"{player.Id} | SCP550 | {player.Nickname}";
            player.DisplayNickname = NewName;

            player.MaxHealth = 500;
            player.Health = 500;

            player.ClearInventory();
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

            if ((ev.Target == SCP550 || ev.Attacker == SCP550) && (ev.Attacker.Team == Team.SCP || ev.Target.Team == Team.SCP) && ev.DamageType.isWeapon)
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

        public void Dieplayer(Player player)
        {
            string NewName = $"{player.Id} | {player.Nickname}";
            player.DisplayNickname = NewName;
            Scp096.TurnedPlayers.Clear();
            Scp173.TurnedPlayers.Clear();
            player.ARueiHint(400, DieHint, 4);

            PlayerEvent.Hurting -= OnPlayerHurting;
            PlayerEvent.Died -= OnDied;

            player = null;
        }
    }
}
