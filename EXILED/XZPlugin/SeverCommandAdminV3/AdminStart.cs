using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System;

namespace DongShanAPI.SCA
{
    public class AdminStart
    {
        public void OnJoinVerifyAdmin(JoinedEventArgs ev)
        {
            Timing.CallDelayed(2f, () =>
            {
                bool IsAdmin = AdminFileManager.IsAdmin(ev.Player.Nickname, ev.Player.IPAddress);
                UserGroup group = ServerStatic.GetPermissionsHandler().GetGroup("admin");
                if (IsAdmin == true)
                {
                    ev.Player.Group = group;
                }
            });
        }
    }
}
