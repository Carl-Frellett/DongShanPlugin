using Exiled.API.Features;
using MEC;
using RueI.Displays;
using RueI.Elements;
using System.Collections.Generic;

namespace DongShanAPI.Hint
{
    public static class AHintAPI
    {
        public static class DisplayManager
        {
            private static Dictionary<ReferenceHub, Display> displays = new Dictionary<ReferenceHub, Display>();

            public static Display GetOrCreateDisplay(ReferenceHub hub)
            {
                if (!displays.ContainsKey(hub))
                {
                    displays[hub] = new Display(hub);
                }
                return displays[hub];
            }
        }

        public static void ARueiHint(this Player player, float initialPosition, string message, int time = 5)
        {
            if (player != null && player.ReferenceHub != null)
            {
                Display display = DisplayManager.GetOrCreateDisplay(player.ReferenceHub);

                float positionModifier = -display.Elements.Count * 30f;

                SetElement setElement = new SetElement(initialPosition + positionModifier, message)
                {
                    Position = initialPosition + positionModifier,
                };

                display.Elements.Add(setElement);

                display.Update();

                Timing.CallDelayed(time, () =>
                {
                    display.Elements.Remove(setElement);
                    display.Update();
                });
            }
        }
    }
}
