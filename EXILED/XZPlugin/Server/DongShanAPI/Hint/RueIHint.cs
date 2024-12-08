using Exiled.API.Features;
using MEC;
using RueI.Displays;
using RueI.Elements;
using System.Collections.Generic;

namespace DongShanAPI.Hint
{
    public static class RueIHint
    {
        public static class DisplayManager
        {
            private static Dictionary<ReferenceHub, Display> 显示 = new Dictionary<ReferenceHub, Display>();

            public static Display GetOrCreateDisplay(ReferenceHub hub)
            {
                if (!显示.ContainsKey(hub))
                {
                    显示[hub] = new Display(hub);
                }
                return 显示[hub];
            }
        }

        public static void RueiHint(this Player player, float 位置, string 文本, int 显示时间 = 5)
        {
            if (player != null && player.ReferenceHub != null)
            {
                Display 显示 = DisplayManager.GetOrCreateDisplay(player.ReferenceHub);

                SetElement 元素 = new SetElement(位置, 文本)
                {
                    Position = 位置,
                };

                显示.Elements.Add(元素);
                显示.Update();

                Timing.CallDelayed(显示时间, () =>
                {
                    显示.Elements.Remove(元素);
                    显示.Update();
                });
            }
        }
    }
}
