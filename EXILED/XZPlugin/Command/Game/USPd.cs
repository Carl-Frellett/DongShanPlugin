using CommandSystem;
using Exiled.API.Extensions;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using System.Linq;

namespace XZPlugin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class UspDamageCommand : ICommand
    {
        public string Command { get; } = "usp";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "显示玩家的USP可以对敌人造成多少伤害";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender is PlayerCommandSender playerSender))
            {
                response = "此命令只能由玩家使用";
                return false;
            }

            Player player = Player.Get(playerSender.Nickname);
            if (player == null)
            {
                response = "错误：找不到命令使用者";
                return false;
            }

            // 检查玩家是否拥有USP
            var uspItem = player.Inventory.items.FirstOrDefault(item => item.id == ItemType.GunUSP);
            if (uspItem == null)
            {
                response = "你没有USP";
                return false;
            }

            int damage = ComputeUSPDamage(player);
            response = $"你的USP可以对敌人造成 {damage} 点伤害。";
            return true;
        }

        private int ComputeUSPDamage(Player player)
        {
            USP u = new USP();
            int defaultDamage = new Random().Next(20, 26);
            if (player != null)
            {
                int 医疗 = player.Inventory.items.Count(item => item.id.IsMedical() && item.id != ItemType.SCP500);
                int 武器 = player.Inventory.items.Count(item => item.id.IsWeapon() && item.id != ItemType.GunUSP);
                int USP = player.Inventory.items.Count(item => item.id == ItemType.GunUSP);
                int 面板 = player.Inventory.items.Count(item => item.id == ItemType.WeaponManagerTablet);
                int 硬币 = player.Inventory.items.Count(item => item.id == ItemType.Coin);
                int 钥匙卡 = player.Inventory.items.Count(item => item.id.IsKeycard());
                int 对讲机 = player.Inventory.items.Count(item => item.id == ItemType.Radio);
                int 电炮 = player.Inventory.items.Count(item => item.id == ItemType.MicroHID);
                int 缴械器 = player.Inventory.items.Count(item => item.id == ItemType.Disarmer);
                int 闪光弹 = player.Inventory.items.Count(item => item.id == ItemType.GrenadeFlash);
                int 手雷 = player.Inventory.items.Count(item => item.id == ItemType.GrenadeFrag);
                int SCPItem = player.Inventory.items.Count(item =>
                    item.id == ItemType.SCP500 ||
                    item.id == ItemType.SCP207 ||
                    item.id == ItemType.SCP018 ||
                    item.id == ItemType.SCP268);

                defaultDamage += u.医疗物品计算(医疗);
                defaultDamage += 武器 > 0 ? 15 : 0;
                if (USP > 1) defaultDamage += 30;
                defaultDamage += 面板 > 0 ? 10 : 0;
                defaultDamage += 硬币 > 0 ? 15 : 0;
                defaultDamage += 钥匙卡 > 0 ? 10 : 0;
                defaultDamage += 对讲机 > 0 ? 10 : 0;
                defaultDamage += 电炮 > 0 ? 40 : 0;
                defaultDamage += 缴械器 > 0 ? 10 : 0;
                defaultDamage += 闪光弹 > 0 ? 15 : 0;
                defaultDamage += 手雷 > 0 ? 15 : 0;

                switch (SCPItem)
                {
                    case 1: defaultDamage += 20; break;
                    case 2: defaultDamage += 30; break;
                    case 3: defaultDamage += 40; break;
                }

                if (defaultDamage >= 250)
                {
                    defaultDamage = 250;
                }
            }

            return defaultDamage;
        }
    }
}
