using DongShanAPI.Hint;
using Exiled.API.Enums;
using DongShanAPI.Item.Spawn;
using DongShanAPI.Item.RomoveItem;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System.Linq;
using UnityEngine;

namespace XZPlugin
{
    internal class Sxy
    {
        private Vector3 OpenSCP1056;

        public void OnRoundStarted()
        {
            Room COMroom = Map.Rooms.FirstOrDefault(room => room.Type == RoomType.EzIntercom);
            if (COMroom != null)
            {
                Vector3 StartPos = COMroom.Position + Vector3.up;
                OpenSCP1056 = StartPos;
                SpawnItems.SpawnPosItem(ItemType.SCP500, StartPos);
            }
        }

        public void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (ev.Pickup.ItemId == ItemType.SCP500)
            {
                if (Vector3.Distance(ev.Pickup.position, OpenSCP1056) < 1.0f)
                {
                    ev.IsAllowed = true;

                    Player player = ev.Player;
                    player.Scale = new Vector3(0.7f, 0.7f, 0.7f);

                    Vector3 UPpos = player.Position + Vector3.up * 0.5f;
                    player.Position = UPpos;

                    player.RueiHint(400, "<align=left><size=25>[SCP1056] 你使用了SCP1056(缩小仪)!</size></align>", 4);

                    Timing.CallDelayed(0.1f, () => RomoveItem.RemoveItem(player, ItemType.SCP500));
                }
                else
                {
                    ev.IsAllowed = true;
                }
            }
        }

        public void OnDied(DiedEventArgs ev)
        {
            ev.Target.Scale = new Vector3(1f, 1f, 1f);
            ev.Target.Position += Vector3.up * 0.5f;
        }

    }
}
