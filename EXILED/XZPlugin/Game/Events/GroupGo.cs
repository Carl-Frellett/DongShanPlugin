using Assets._Scripts.Dissonance;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System.Linq;
using UnityEngine;

namespace XZPlugin
{
    class GroupGo
    {
        public void GroupEps(EscapingEventArgs ev)
        {
            if (ev.Player == null)
            {
                return;
            }
            else
            {
                if (ev.Player.Role == RoleType.FacilityGuard && !ev.Player.IsCuffed)
                {
                    ev.IsAllowed = true;
                    ev.NewRole = RoleType.NtfScientist;
                }
            }
        }
    }
}
