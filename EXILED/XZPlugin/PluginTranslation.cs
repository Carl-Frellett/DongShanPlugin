using Exiled.API.Interfaces;
using System.ComponentModel;

namespace XZPlugin
{
    public class Translation : ITranslation
    {
        [Description("等待大厅显示的文本:")]
        public string TopMessage { get; set; } = "<align=center><size=40><color=yellow><b>回合即将开始, [ %seconds]</b></color></size></align>";
        public string BottomMessage { get; set; } = "<align=center><size=30><i>已有 %players </i></size><size=40><b></align>\n<align=center><align=center><size=30><color=red></color></size></align></align>";
        public string ServerIsPaused { get; set; } = "已暂停 ";
        public string RoundIsBeingStarted { get; set; } = "即将开始 ";
        public string OneSecondRemain { get; set; } = "";
        public string XSecondsRemains { get; set; } = "";
        public string OnePlayerConnected { get; set; } = "名玩家加入了服务器";
        public string XPlayersConnected { get; set; } = "名玩家加入了服务器";
    }
}
