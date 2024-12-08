using Exiled.API.Features;
using System.Linq;

namespace DongShanAPI.Item.RomoveItem
{
    public static class RomoveItem
    {
        public static void RemoveItem(Player player, ItemType itemType)
        {
            var item = player.Inventory.items.FirstOrDefault(i => i.id == itemType);
            if (!item.Equals(default))
            {
                player.RemoveItem(item);
            }
        }
    }
}
