using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace XZPlugin
{
    public class InfinityAmmo
    {
        public void OnShooting(ShootingEventArgs ev)
        {
            SetAmmoToMax(ev.Shooter, 114514);
        }

        public void OnReloading(ReloadingWeaponEventArgs ev)
        {
            SetAmmoToMax(ev.Player, 114514);
        }
        public void Dying(DyingEventArgs ev)
        {
            SetAmmoToMax(ev.Target, 0);
        }
        public void SetAmmoToMax(Player player, uint ammo)
        {
            if (player.CurrentItem != null && player.CurrentItem.id != ItemType.None)
            {
                foreach (AmmoType Type in System.Enum.GetValues(typeof(AmmoType)))
                {
                    int i = (int)Type;
                    player.Ammo[i] = ammo;
                }
            }
        }
    }
}