using DongShanAPI.Hint;
using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XZPlugin
{
    public class TX_Event
    {
        public void Hurt(HurtingEventArgs ev)
        {
            if ((ev.Target.Role == RoleType.ClassD && ev.Target.RankName.Contains("投降人员") == true) && (ev.Target.Team != Team.SCP && ev.Target.Team != Team.CDP) && ev.DamageType.isWeapon)
            {
                ev.IsAllowed = false;
                ev.Attacker.RueiHint(300,"<b><i>你不可以攻击投降人员</i></b>",1);
            }
            if ((ev.Attacker.Role == RoleType.ClassD && ev.Attacker.RankName.Contains("投降人员")) && (ev.Target.Team != Team.SCP && ev.Target.Team != Team.CDP))
            {
                ev.IsAllowed = false;
                ev.Attacker.RueiHint(300, "<b><i>作为投降人员你不可以攻击人类</i></b>", 1);
            }
            else
            {
                ev.IsAllowed = true;
            }
        }
        public void Die(DyingEventArgs ev)
        {
            if (ev.Target.Role == RoleType.ClassD && ev.Target.RankName.Contains("投降人员"))
            {
               ev.Target.RankName.Replace("投降人员", "");
            }
            if (ev.Target.Role == RoleType.ClassD && ev.Target.RankName.Contains(" | 投降人员"))
            {
                ev.Target.RankName.Replace(" | 投降人员", "");
            }
            else
            {
                ev.IsAllowed = true;
            }
        }
        public void OnEsp(EscapingEventArgs ev)
        {
            if (ev.Player.Role == RoleType.ClassD && ev.Player.RankName.Contains("投降人员"))
            {
                ev.Player.RankName.Replace("投降人员", "");
                ev.NewRole = RoleType.NtfCadet;
            }
            if (ev.Player.Role == RoleType.ClassD && ev.Player.RankName.Contains(" | 投降人员"))
            {
                ev.Player.RankName.Replace(" | 投降人员", "");
                ev.NewRole = RoleType.NtfCadet;
            }
            else
            {
                ev.IsAllowed = true;
            }
        }
        public void OnChange(ChangedRoleEventArgs ev)
        {
            if (ev.Player.Role == RoleType.ClassD && ev.Player.RankName.Contains("投降人员"))
            {
                ev.Player.RankName.Replace("投降人员", "");
            }
            if (ev.Player.Role == RoleType.ClassD && ev.Player.RankName.Contains(" | 投降人员"))
            {
                ev.Player.RankName.Replace(" | 投降人员", "");
            }
        }
    }
}
