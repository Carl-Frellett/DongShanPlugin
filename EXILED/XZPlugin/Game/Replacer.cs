using DongShanAPI.Hint;
using Exiled.API.Extensions;
using Exiled.Events.EventArgs;
using MEC;
using System.Linq;
using UnityEngine;

namespace XZPlugin
{
    class Replacer
    {
        public void OnLeave(LeftEventArgs ev)
        {
            // 当离开的玩家为教程角色时，不进行替换
            if (ev.Player.Role == RoleType.Tutorial)
                return;

            if (ev.Player.Role.GetTeam() != Team.RIP)
            {
                bool scp079 = false;
                if (ev.Player.Role == RoleType.Scp079)
                    scp079 = true;

                Inventory.SyncListItemInfo items = ev.Player.Inventory.items;
                RoleType role = ev.Player.Role;
                Vector3 pos = ev.Player.Position;
                float health = ev.Player.Health, xp079 = 0f, ap079 = 0f;
                byte lvl079 = 0;

                if (scp079)
                {
                    lvl079 = ev.Player.Level;
                    xp079 = ev.Player.Experience;
                    ap079 = ev.Player.Energy;
                }

                Exiled.API.Features.Player player = Exiled.API.Features.Player.List.FirstOrDefault(x => x.Role == RoleType.Spectator && x.UserId != string.Empty && !x.IsOverwatchEnabled && x != ev.Player);

                if (player != null)
                    player.SetRole(role);

                Timing.CallDelayed(0.3f, () =>
                {
                    player.Position = pos;
                    foreach (var item in items)
                        player.Inventory.AddNewItem(item.id);
                    player.Health = health;

                    if (scp079)
                    {
                        player.Level = lvl079;
                        player.Experience = xp079;
                        player.Energy = ap079;
                    }
                    player.RueiHint(400, $"<align=left><size=25>[个人消息] <color=yellow>你因一名玩家的离开而被补位</color></size></align>", 4);
                    player.SendConsoleMessage("你因一名玩家离开服务器而因此补位", "red");
                });
            }
        }
    }
}
