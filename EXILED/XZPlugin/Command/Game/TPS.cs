 using CommandSystem;
using System;
using UnityEngine;

namespace XZPlugin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class TpsCommand : ICommand
    {
        public string Command { get; } = "Tps";
        public string[] Aliases { get; } = Array.Empty<string>();
        public string Description { get; } = "查询服务器的基本信息";
        public static double Tps => Math.Round(1f / Time.smoothDeltaTime);
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = $"当前服务器TPS: {(int)Tps}";
            return false;
        }
    }
}
