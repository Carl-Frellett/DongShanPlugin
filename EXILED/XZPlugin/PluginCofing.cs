using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace XZPlugin
{
    public class Config : IConfig
    {
        [Description("插件开关")]
        public bool IsEnabled { get; set; } = true;

        [Description("Debug调试")]
        public bool DEBUG { get; set; } = false;

        [Description("确定是否将显示任何类型的消息")]
        public bool DisplayWaitMessage { get; set; } = true;

        [Description("SCP站多久回血？")]
        public float SCP_KeepPosTime { get; set; } = 5;

        [Description("SCP回多少血？")]
        public float SCP_Health { get; set; } = 5;

        [Description("玩家该变成什么:")]
        public List<RoleType> RolesToChoose { get; set; } = new List<RoleType>
        {
            RoleType.Tutorial,
        };

        [Description("是否允许玩家在大厅内攻击:")]
        public bool AlowDamage { get; set; } = false;

        [Description("是否禁止玩家激怒096和定住SCP173:")]
        public bool TurnedPlayers { get; set; } = true;

        [Description("在大厅中给玩家一个SCP-207的效果：（设置0禁用）")]
        public byte ColaMultiplier { get; set; } = 100;
    }
}
