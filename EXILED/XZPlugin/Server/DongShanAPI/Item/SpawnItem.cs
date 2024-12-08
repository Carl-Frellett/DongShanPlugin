using Exiled.API.Extensions;
using Exiled.API.Features;
using Mirror;
using UnityEngine;

namespace DongShanAPI.Item.Spawn
{
    public static class SpawnItems
    {
        public static void SpawnItem(this Player player, ItemType itemType, float x, float y, float z)
        {
            if (player.Role != RoleType.Spectator && player.Role != RoleType.None)
            {
                SetSpawnScaleItem(player, itemType, new Vector3(x, y, z));
            }
        }
        public static void SpawnPosItem(ItemType type, Vector3 position, int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 spawnPosition = position + Vector3.right * (i * 1.5f);
                _ = type.Spawn(0, spawnPosition, Quaternion.identity, 0, 0, 0);
            }
        }
        public static void SetSpawnScaleItem(Player player, ItemType itemType, Vector3 scale)
        {
            Vector3 spawnPosition = player.Position;
            Pickup item = itemType.Spawn(0, spawnPosition, Quaternion.identity);

            GameObject gameObject = item.gameObject;
            gameObject.transform.localScale = scale;

            NetworkServer.UnSpawn(gameObject);
            NetworkServer.Spawn(gameObject);
        }
    }
}
