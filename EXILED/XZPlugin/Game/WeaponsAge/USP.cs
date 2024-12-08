using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Linq;

namespace XZPlugin
{
    public class USP
    {
        private readonly Random random = new Random();
        private HitBoxType Hitbox;

        public void OnPlayerHurting(HurtingEventArgs ev)
        {
            if (ev.DamageType == DamageTypes.Usp)
            {
                int defaultDamage = random.Next(20, 26);

                if (ev.Attacker != null)
                {
                    int 医疗 = ev.Attacker.Inventory.items.Count(item => item.id.IsMedical() && item.id != ItemType.SCP500);
                    int 武器 = ev.Attacker.Inventory.items.Count(item => item.id.IsWeapon() && item.id != ItemType.GunUSP);
                    int USP = ev.Attacker.Inventory.items.Count(item => item.id == ItemType.GunUSP);
                    int 面板 = ev.Attacker.Inventory.items.Count(item => item.id == ItemType.WeaponManagerTablet);
                    int 硬币 = ev.Attacker.Inventory.items.Count(item => item.id == ItemType.Coin);
                    int 钥匙卡 = ev.Attacker.Inventory.items.Count(item => item.id.IsKeycard());
                    int 对讲机 = ev.Attacker.Inventory.items.Count(item => item.id == ItemType.Radio);
                    int 电炮 = ev.Attacker.Inventory.items.Count(item => item.id == ItemType.MicroHID);
                    int 缴械器 = ev.Attacker.Inventory.items.Count(item => item.id == ItemType.Disarmer);
                    int 闪光弹 = ev.Attacker.Inventory.items.Count(item => item.id == ItemType.GrenadeFlash);
                    int 手雷 = ev.Attacker.Inventory.items.Count(item => item.id == ItemType.GrenadeFrag);
                    int SCPItem = ev.Attacker.Inventory.items.Count(item =>
                        item.id == ItemType.SCP500 ||
                        item.id == ItemType.SCP207 ||
                        item.id == ItemType.SCP018 ||
                        item.id == ItemType.SCP268);

                    //持有是指背包内有并非手中
                    defaultDamage += 医疗物品计算(医疗);  // 医疗物品伤害计算 1个 +10，2个 +15，3个 +20
                    defaultDamage += 武器 > 0 ? 15 : 0;  //持有武器计算  仅算一把武器的伤害 一把 +15
                    defaultDamage += 面板 > 0 ? 10 : 0;  //持有武器面板计算  +10
                    defaultDamage += 硬币 > 0 ? 15 : 0;  //持有硬币计算 仅限一枚 +15
                    defaultDamage += 钥匙卡 > 0 ? 10 : 0;  //持有钥匙卡计算 仅限一张 +10
                    defaultDamage += 对讲机 > 0 ? 10 : 0;  //持有对讲机计算 仅限一个 +10
                    defaultDamage += 电炮 > 0 ? 40 : 0;  //持有HID计算 +40
                    defaultDamage += 缴械器 > 0 ? 10 : 0;  //持有缴械器计算 仅限一个 +10
                    defaultDamage += 闪光弹 > 0 ? 15 : 0;  //持有闪光弹计算 仅限一个 +15
                    defaultDamage += 手雷 > 0 ? 15 : 0;  //持有手雷计算 仅限一个 +15
                    if (USP > 1) defaultDamage += 30;  //持有USP计算 仅限一个 +30


                    switch (SCPItem)  //SCP物品伤害加成
                    {
                        case 1:
                            defaultDamage += 20;
                            break;
                        case 2:
                            defaultDamage += 30;
                            break;
                        case 3:
                            defaultDamage += 40;
                            break;
                    }

                    if (defaultDamage >= 60)  //仅限当伤害大于等于40时才会启用，以确保伤害不会过于逆天
                    {
                        switch (Hitbox)
                        {
                            case HitBoxType.BODY:
                                defaultDamage -= 22;  //击中胳膊会导致无法射击，因此默认伤害减22
                                break;
                            case HitBoxType.ARM:
                                defaultDamage -= 16;  //击中胳膊会导致无法射击，因此默认伤害减16
                                break;
                            case HitBoxType.LEG:
                                defaultDamage -= 14;  //击中腿部会导致无法走路，因此默认伤害减14
                                break;
                        }
                    }
                    if (defaultDamage <= 60)  //同理，如果伤害小于等于60，则进行加强伤害
                    {
                        switch (Hitbox)
                        {
                            case HitBoxType.HEAD:
                                defaultDamage += 20;  //头部
                                break;
                            case HitBoxType.BODY:
                                defaultDamage += 14;  //身体
                                break;
                            case HitBoxType.ARM:
                                defaultDamage += 8;  //胳膊
                                break;
                            case HitBoxType.LEG:
                                defaultDamage += 8;  //腿
                                break;
                        }
                    }

                    if (ev.Target.Role == RoleType.Scp106)
                    {
                        defaultDamage = 10;  //SCP106的抗性
                    }
                    else if (ev.Target.Team == Team.SCP && ev.Target.Role != RoleType.Scp106)
                    {
                        defaultDamage = 25;  //防止对SCP造成过多的伤害
                    }


                    if (defaultDamage >= 250)
                    {
                        defaultDamage = 250;  // 防止伤害超出过多的值
                    }
                    if (defaultDamage <= 20)
                    {
                        defaultDamage = random.Next(20, 26); //防止伤害超出默认值
                    }

                    Log.Info($"USP攻击造成伤害: {defaultDamage}");

                    ev.Amount = defaultDamage;
                }
                else
                {
                    Log.Info("攻击者不存在，无法计算伤害调整");
                }
            }
        }

        public int 医疗物品计算(int count)
        {
            if (count == 1) return 10;
            if (count == 2) return 15;
            if (count >= 3) return 20;
            return 0;
        }
    }
}