using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace XZPlugin
{
    class InventoryAccess
    {
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Player.InteractingDoor += OnPlayerDoorInteract;
            Exiled.Events.Handlers.Player.InteractingLocker += OnPlayerLockerInteract;
            Exiled.Events.Handlers.Player.UnlockingGenerator += OnGeneratorAccess;
            Exiled.Events.Handlers.Player.ActivatingWarheadPanel += OnActivatingWarheadPanel;
        }

        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Player.InteractingDoor -= OnPlayerDoorInteract;
            Exiled.Events.Handlers.Player.InteractingLocker -= OnPlayerLockerInteract;
            Exiled.Events.Handlers.Player.UnlockingGenerator -= OnGeneratorAccess;
            Exiled.Events.Handlers.Player.ActivatingWarheadPanel -= OnActivatingWarheadPanel;
        }
        public void OnPlayerDoorInteract(InteractingDoorEventArgs ev)
        {
            if (ev.Player.Side == Side.Scp) return;
            if (ev.IsAllowed) return;
#pragma warning disable CS0618 
            ev.IsAllowed = HasPermission(ev.Player, ev.Door.permissionLevel);
#pragma warning restore CS0618
        }

        public void OnPlayerLockerInteract(InteractingLockerEventArgs ev)
        {
            if (ev.Player.Side == Side.Scp) return;
            if (ev.IsAllowed) return;

            ev.IsAllowed = HasPermission(ev.Player, ev.Chamber.accessToken);
        }

        public void OnGeneratorAccess(UnlockingGeneratorEventArgs ev)
        {
            if (ev.Player.Side == Side.Scp) return;
            if (ev.IsAllowed) return;
            ev.IsAllowed = HasPermission(ev.Player, "ARMORY_LVL_2");
        }

        public void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            ev.IsAllowed = false;

            if (ev.Player.IsBypassModeEnabled)
            {
                ev.IsAllowed = true;
                return;
            }

            if (ev.Player.Side == Side.Scp) return;

            ev.IsAllowed = HasPermission(ev.Player, "CONT_LVL_3");
        }

        private bool HasPermission(Player player, string requested)
        {
            if (requested == "")
            {
                return true;
            }

            foreach (var item in player.Inventory.items)
            {
                foreach (var permission in player.Inventory.GetItemByID(item.id).permissions)
                {
                    if (requested.Contains(permission))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
