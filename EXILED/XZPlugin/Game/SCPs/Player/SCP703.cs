using DongShanAPI.Hint;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace XZPlugin
{
    public class S703
    {
        string DieHint = "<align=left><color=yellow><size=25>[收容通知]</color><color=red>SCP-703现已被收容</color></size></align>";
        string PlayerHint = "<size=25>[个人消息] 703将在3秒后刷新物品! ";
        string StartHint = "<b>你是\n<size=100><color=red>SCP-703</color></size>\n每过3分钟刷东西</b>";

        public Player SCP703;
        public readonly List<CoroutineHandle> coroutine = new List<CoroutineHandle>();
        public readonly Random random = new Random();

        public void OnRoundStarted()
        {
            Timing.CallDelayed(2f, () =>
            {
                StartSCP703();
            });
        }

        public void StartSCP703()
        {
            var ply = Player.List.Where(p => p.Role == RoleType.Scientist && p.IsAlive).ToList();
            if (ply.Count > 0)
            {
                SCP703 = ply[random.Next(ply.Count)];
                if (SCP703 != null)
                {
                    string NewNmae = $"{SCP703.Id} | SCP703 | {SCP703.Nickname}";
                    SCP703.DisplayNickname = NewNmae;
                    SCP703.RueiHint(400, StartHint, 10);
                    coroutine.Add(Timing.RunCoroutine(ItemGivingRoutine()));
                }
            }
        }
        public void OnPlayerDied(DiedEventArgs ev)
        {
            if (ev.Target != null && ev.Target == SCP703)
            {
                DieSCP703();
            }
        }

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player != null && ev.Player == SCP703)
            {
                DieSCP703();
            }
        }
        public void DieSCP703()
        {
            if (SCP703 != null)
            {
                string NewName = $"{SCP703.Id} | {SCP703.Nickname}";
                SCP703.DisplayNickname = NewName;
                SCP703 = null;
                StopCoroutines();
            }
            foreach (Player p in Player.List)
            {
                p.ARueiHint(400, DieHint, 5);
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
        public IEnumerator<float> ItemGivingRoutine()
        {
            while (SCP703 != null)
            {
                yield return Timing.WaitForSeconds(180 - 3);

                SCP703.RueiHint(400, PlayerHint, 3);

                yield return Timing.WaitForSeconds(3);

                if (SCP703 != null && SCP703.IsAlive && Player.List.Contains(SCP703))
                {
                    RandomItem(SCP703);
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
