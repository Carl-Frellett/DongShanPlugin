using DongShanAPI.Hint;
using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace XZPlugin
{
    public class SCP914
    {
        public void On914(ActivatingEventArgs ev)
        {
            string mode = Mode914(Exiled.API.Features.Scp914.KnobStatus);

            foreach (var player in Player.List)
            {
                if (player.CurrentRoom.Type == Exiled.API.Enums.RoomType.Lcz914)
                {
                    player.RueiHint(380, $"<size=25>[SCP914] \n{ev.Player.Nickname} 使用了SCP914\n914加工模式: <b>{mode}</b></size>", 7);
                    player.SendConsoleMessage($"[SCP914] 玩家{player.Nickname}使用了SCP914 -模式{mode}", "#808080");
                }
            }
        }

        private string Mode914(Scp914.Scp914Knob mode)
        {
            switch (mode)
            {
                case Scp914.Scp914Knob.Rough:
                    return "超粗";
                case Scp914.Scp914Knob.Coarse:
                    return "粗加工";
                case Scp914.Scp914Knob.OneToOne:
                    return "1:1";
                case Scp914.Scp914Knob.Fine:
                    return "精加工";
                case Scp914.Scp914Knob.VeryFine:
                    return "超精";
                default:
                    return "未知模式";
            }
        }
    }
}
