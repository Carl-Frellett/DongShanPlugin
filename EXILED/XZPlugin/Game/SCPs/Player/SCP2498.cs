using DongShanAPI.Hint;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System;
using System.Linq;

namespace XZPlugin
{
    public class S2498
    {
        string DieHint = "<align=left><color=yellow><size=25>[收容通知]</color><color=red>SCP-2498现已被收容</color></size></align>";
        string StartHint = "<b>你是\n<size=100><color=red>SCP-2498</color></size>\n透视</b>";
        public Player SCP2498;

        public void OnRoundStarted()
        {
            Timing.CallDelayed(2f, () =>
            {
                var ply = Player.List.Where(p => p.Role == RoleType.ClassD).ToList();
                var Eply = ply.Where(p => !p.DisplayNickname.Contains("SCP181") && !p.DisplayNickname.Contains("SCP457") && !p.DisplayNickname.Contains("SCP550")).ToList();

                if (Eply.Any())
                {
                    SCP2498 = Eply[new Random().Next(Eply.Count)];
                    SCP2498.ReferenceHub.playerEffectsController.EnableEffect<CustomPlayerEffects.Visuals939>();
                    SCP2498.RueiHint(400, StartHint, 10);

                    string NewName = $"{SCP2498.Id} | SCP2498 | {SCP2498.Nickname}";
                    SCP2498.DisplayNickname = NewName;
                }
            });
        }

        public void OnPlayerDied(DiedEventArgs ev)
        {
            if (ev.Target == SCP2498 && SCP2498 != null)
            {
                DieSCP2498(SCP2498);
            }
        }

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player == SCP2498 && SCP2498 != null)
            {
                DieSCP2498(SCP2498);
            }
        }

        private void DieSCP2498(Player player)
        {
            player.ReferenceHub.playerEffectsController.DisableEffect<CustomPlayerEffects.Visuals939>();

            string NewName = $"{SCP2498.Id} | {SCP2498.Nickname}";
            SCP2498.DisplayNickname = NewName;

            foreach (Player p in Player.List)
            {
                p.ARueiHint(300, DieHint, 5);
            }

            SCP2498 = null;
        }
    }
}
