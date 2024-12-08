using Exiled.API.Features;
using UnityEngine;
using DongShanAPI.Badge.RTags.UnityScript;

namespace DongShanAPI.Badge.RTags
{
    public static class RTags
    {
        private static readonly string[] c = new[]
        {
            "pink", "red", "brown", "silver",
            "light_green", "crimson", "cyan",
            "aqua","deep_pink","tomato",
            "yellow","magenta","blue_green",
            "orange","lime","green",
            "emerald","carmine","nickel",
              "mint","army_green","pumpkin"
        };

        public static void RTag(this Player player, string Name, bool IsEnabled)
        {
            if (player.RankName == "管理员")
            {
                player.RankName = $"管理员 | {Name}";
            }
            else
            {
                player.RankName = Name;
            }

            if (!IsEnabled)
            {
                var o1 = player.GameObject.GetComponent<TagController>();
                if (o1 != null)
                {
                    Object.Destroy(o1);
                }

                player.RankColor = "red";

                return;
            }

            var o2 = player.GameObject.GetComponent<TagController>();

            if (o2 == null)
            {
                o2 = player.GameObject.AddComponent<TagController>();
                o2.Colors = c;
                o2.Interval = 0.5f;
            }
        }
    }
}
