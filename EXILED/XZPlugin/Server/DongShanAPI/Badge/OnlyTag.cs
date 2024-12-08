using Exiled.API.Features;

namespace DongShanAPI.Badge.OnlyTag
{
    public static class OnlyTag
    {
        public static void ORTag(this Player player, string Name, string Color)
        {
            if (player.RankName == "管理员")
            {
                player.RankName = $"管理员 | {Name}";
            }
            else
            {
                player.RankName = Name;
            }
            player.RankColor = Color;
        }
    }
}
