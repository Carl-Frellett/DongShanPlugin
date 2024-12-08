using DongShanAPI.Hint;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using XZPlugin.PluginHarmony;
using DongShanAPI.SCA;
using HarmonyLib;
using MEC;
using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
using Player = Exiled.API.Features.Player;
using PlayerEvent = Exiled.Events.Handlers.Player;
using S914 = Exiled.Events.Handlers.Scp914;
using ServerEvent = Exiled.Events.Handlers.Server;
using WarheadEvent = Exiled.Events.Handlers.Warhead;
using MapEvent = Exiled.Events.Handlers.Map;
using System.Linq;

namespace XZPlugin
{
    public class Plugin : Plugin<Config, Translation>
    {
        private Plugin() { }
        public override string Name => "XZPlugin";
        public override string Author => "Carl Frellet";
        public override System.Version Version => base.Version;
        public string Ver_API { get; set; } ="3.0.0";
        public string ServerName { get; set; } = "東山";
        public string Ver_Game { get; set; } = "10.1.0";
        public string Ver_ServerCommandAdmin { get; set; } = "3.0.0";

        public override PluginPriority Priority => PluginPriority.Higher;
        //插件
        public static Plugin Instance => LazyInstance.Value;  //静态连接配置
        private static readonly Lazy<Plugin> LazyInstance = new Lazy<Plugin>(() => new Plugin()); //连接构造函数
        //SCP站立回血
        private CoroutineHandle SCPHealthCoroutine;
        private Dictionary<Player, Vector3> LastPos = new Dictionary<Player, Vector3>();
        private Dictionary<Player, float> KeepPosTime = new Dictionary<Player, float>();
        //清理
        private CoroutineHandle CleanCoroutines;
        private List<float> CleanCorpsesTime = new List<float> { 300f, 780f, 1200f, 2000f, 2500f };
        private readonly float CleanItemTime = 900f;

        private static Harmony HarmonyInstance { get; set; }
        private SCP181Handler S8;
        private S2498 S9;
        private S703 S7;
        private CoinLikee CoinLikee;
        private Kongtou kongtou;
        private SEe se;
        private WaitingPlayer handler;
        private Sxy sxy;
        private Bright Brit;
        private USP usp;
        private InfinityAmmo IA;
        private SCPINFO SI;
        private Replacer R;
        private InventoryAccess IAS;
        private SCP914 s91;
        private GroupGo GG;
        private AdminStart admin;
        private S6000 S6;
        private S550 S5;
        private S682 S68;
        private TeamNum team;
        private TX_Event tx;


        public override void OnEnabled()
        {
            base.OnEnabled();

            HarmonyInstance = new Harmony($"{Name}");
            HarmonyInstance.PatchAll();

            // InventoryAccess
            IAS = new InventoryAccess();
            IAS.RegisterEvents();
            // Replacer

            R = new Replacer();
            PlayerEvent.Left += R.OnLeave;
            //  队友数量

            team = new TeamNum();
            PlayerEvent.Joined += team.OnJoin;
            ServerEvent.RoundStarted += team.OnRoundStarted;
            //   DD投降

            tx = new TX_Event();
            PlayerEvent.Escaping += tx.OnEsp;
            PlayerEvent.Dying += tx.Die;
            PlayerEvent.ChangedRole += tx.OnChange;

            // 保安下班
            GG = new GroupGo();
            PlayerEvent.Escaping += GG.GroupEps;

            //  管理员自动上权
            admin = new AdminStart();
            PlayerEvent.Joined += admin.OnJoinVerifyAdmin;

            //  SCP6000
            S6 = new S6000();
            PlayerEvent.ChangingRole += S6.OnChangeRole;
            PlayerEvent.Died += S6.OnDie;
            PlayerEvent.DroppingItem += S6.OnDropItem;
            ServerEvent.RoundStarted += S6.OnRoundStart;

            //  SCP682
            S68 = new S682();
            ServerEvent.RoundStarted += S68.OnRoundStart;

            //  SCP550
            S5 = new S550();
            ServerEvent.RespawningTeam += S5.OnRespawning;

            // 等待玩家事件
            handler = new WaitingPlayer();
            ServerEvent.WaitingForPlayers += handler.OnWaitingForPlayers;
            PlayerEvent.Joined += handler.OnPlayerJoin;
            ServerEvent.RoundStarted += handler.OnRoundStarted;

            //  称号
            //Only.Reg();
            //Raw.Reg();

            //USP加强
            usp = new USP();
            PlayerEvent.Hurting += usp.OnPlayerHurting;

            //缩小仪
            sxy = new Sxy();
            ServerEvent.RoundStarted += sxy.OnRoundStarted;
            PlayerEvent.PickingUpItem += sxy.OnPickingUpItem;
            PlayerEvent.Died += sxy.OnDied;

            //无限子弹
            IA = new InfinityAmmo();
            PlayerEvent.Shooting += IA.OnShooting;
            PlayerEvent.ReloadingWeapon += IA.OnReloading;
            PlayerEvent.Dying += IA.Dying;

            //SCP显示血量
            SI = new SCPINFO();
            PlayerEvent.ChangingRole += SI.OnChangingRole;

            //硬币抽奖
            CoinLikee = new CoinLikee();
            PlayerEvent.DroppingItem += CoinLikee.OnItemDropping;

            //空投
            kongtou = new Kongtou();
            ServerEvent.RespawningTeam += kongtou.OnRespawningTeam;

            //随机事件
            se = new SEe();
            ServerEvent.RoundStarted += se.OnRoundStarted;
            PlayerEvent.Died += se.OnPlayerDied;
            Timing.KillCoroutines(se.Coroutine);

            //亮亮
            Brit = new Bright();
            ServerEvent.RespawningTeam += Brit.OnRespawning;
            PlayerEvent.Died += Brit.OnPlayerDied;

            //  SCP181
            S8 = new SCP181Handler();
            PlayerEvent.Hurting += S8.OnPlayerHurting;
            ServerEvent.RoundStarted += S8.OnRoundStarted;
            PlayerEvent.Died += S8.OnPlayerDied;
            PlayerEvent.ChangingRole += S8.OnChangingRole;
            PlayerEvent.InteractingDoor += S8.OnInteractingDoor;
            PlayerEvent.InteractingLocker += S8.OnInteractingLocker;
            PlayerEvent.UnlockingGenerator += S8.UnlockingGenerator;
            PlayerEvent.ActivatingWarheadPanel += S8.OnActivatingWarheadPanel;

            // SCP2498
            S9 = new S2498();
            ServerEvent.RoundStarted += S9.OnRoundStarted;
            PlayerEvent.Died += S9.OnPlayerDied;
            PlayerEvent.ChangingRole += S9.OnChangingRole;

            // SCP703
            S7 = new S703();
            ServerEvent.RoundStarted += S7.OnRoundStarted;
            PlayerEvent.Died += S7.OnPlayerDied;
            PlayerEvent.ChangingRole += S7.OnChangingRole;

            // SCP914
            s91 = new SCP914();
            S914.Activating += s91.On914;

            // 事件注册
            ServerEvent.RoundStarted += OnRoundStarted;
            PlayerEvent.ChangingRole += OnChangingRole;
            PlayerEvent.Hurting += OnPlayerHurting;
            ServerEvent.RespawningTeam += OnRespawningTeam;
            PlayerEvent.Dying += OnPlayerDying;
            ServerEvent.RoundEnded += OnRoundEnded;
            WarheadEvent.Starting += OnWarheadStarting;
            WarheadEvent.Stopping += OnWarheadStopping;
            WarheadEvent.Detonated += OnWarheadDetonated;
            MapEvent.Decontaminating += OnDecontaminating;
            Exiled.Events.Handlers.Scp096.Enraging += OnSawSCP096;
            SCPHealthCoroutine = Timing.RunCoroutine(SCPHealth());
            LogInitialization();
        }

        public override void OnDisabled()
        {
            base.OnDisabled();
            TX tX = new TX();
            Timing.KillCoroutines(SCPHealthCoroutine); //SCP站立回血
            Timing.KillCoroutines(CleanCoroutines);// 清理

            HarmonyInstance.UnpatchAll();

            ServerEvent.RoundStarted -= OnRoundStarted;
            PlayerEvent.ChangingRole -= OnChangingRole;
            PlayerEvent.Hurting -= OnPlayerHurting;
            ServerEvent.RespawningTeam -= OnRespawningTeam;
            PlayerEvent.Dying -= OnPlayerDying;
            ServerEvent.RoundEnded -= OnRoundEnded;
            WarheadEvent.Starting -= OnWarheadStarting;
            WarheadEvent.Stopping -= OnWarheadStopping;
            WarheadEvent.Detonated -= OnWarheadDetonated;
            MapEvent.Decontaminating -= OnDecontaminating;
            Exiled.Events.Handlers.Scp096.Enraging -= OnSawSCP096;
            // Replacer

            R = null;
            PlayerEvent.Left -= R.OnLeave;
            //   DD投降

            tx = null;
            PlayerEvent.Escaping -= tx.OnEsp;
            PlayerEvent.Dying -= tx.Die;
            PlayerEvent.ChangedRole -= tx.OnChange;

            //  称号
            //Only.Stop();
            //Raw.Stop();

            // 保安下班
            GG = null;
            PlayerEvent.Escaping -= GG.GroupEps;

            //  队友数量
            team = null;
            PlayerEvent.Joined -= team.OnJoin;
            ServerEvent.RoundStarted -= team.OnRoundStarted;
            // InventoryAccess

            IAS = null;
            IAS.UnregisterEvents();
            //  管理员自动上权

            admin = null ;
            PlayerEvent.Joined -= admin.OnJoinVerifyAdmin;
            //  SCP682

            S68 = new S682();
            ServerEvent.RoundStarted += S68.OnRoundStart;
            // SCP914

            s91 = null;
            S914.Activating -= s91.On914;
            //  SCP550

            S5 = null;
            ServerEvent.RespawningTeam -= S5.OnRespawning;
            // 等待玩家事件

            handler = null;
            ServerEvent.WaitingForPlayers -= handler.OnWaitingForPlayers;
            PlayerEvent.Joined -= handler.OnPlayerJoin;
            ServerEvent.RoundStarted -= handler.OnRoundStarted;
            //  SCP6000

            S6 = null;
            PlayerEvent.ChangingRole -= S6.OnChangeRole;
            PlayerEvent.Died -= S6.OnDie;
            PlayerEvent.DroppingItem -= S6.OnDropItem;
            ServerEvent.RoundStarted -= S6.OnRoundStart;

            //USP加强

            usp = null;
            PlayerEvent.Hurting -= usp.OnPlayerHurting;
            //缩小仪

            sxy = null;
            ServerEvent.RoundStarted -= sxy.OnRoundStarted;
            PlayerEvent.PickingUpItem -= sxy.OnPickingUpItem;
            PlayerEvent.Died -= sxy.OnDied;
            //无限子弹

            IA = null;
            PlayerEvent.Shooting -= IA.OnShooting;
            PlayerEvent.ReloadingWeapon -= IA.OnReloading;
            PlayerEvent.Dying -= IA.Dying;
            //SCP显示血量

            SI = null;
            PlayerEvent.ChangingRole -= SI.OnChangingRole;
            //硬币抽奖

            CoinLikee = null;
            PlayerEvent.DroppingItem -= CoinLikee.OnItemDropping;
            //空投

            kongtou = null;
            ServerEvent.RespawningTeam -= kongtou.OnRespawningTeam;
            //随机事件

            se = null;
            ServerEvent.RoundStarted -= se.OnRoundStarted;
            PlayerEvent.Died -= se.OnPlayerDied;
            Timing.KillCoroutines(se.Coroutine);
            //亮亮

            Brit = null;
            ServerEvent.RespawningTeam -= Brit.OnRespawning;
            PlayerEvent.Died -= Brit.OnPlayerDied;
            //  SCP181

            S8 = null;
            PlayerEvent.Hurting -= S8.OnPlayerHurting;
            ServerEvent.RoundStarted -= S8.OnRoundStarted;
            PlayerEvent.Died -= S8.OnPlayerDied;
            PlayerEvent.ChangingRole -= S8.OnChangingRole;
            PlayerEvent.InteractingDoor -= S8.OnInteractingDoor;
            PlayerEvent.InteractingLocker -= S8.OnInteractingLocker;
            PlayerEvent.UnlockingGenerator -= S8.UnlockingGenerator;
            PlayerEvent.ActivatingWarheadPanel -= S8.OnActivatingWarheadPanel;
            // SCP2498

            S9 = null;
            ServerEvent.RoundStarted -= S9.OnRoundStarted;
            PlayerEvent.Died -= S9.OnPlayerDied;
            PlayerEvent.ChangingRole -= S9.OnChangingRole;
            // SCP703

            S7 = null;
            ServerEvent.RoundStarted -= S7.OnRoundStarted;
            PlayerEvent.Died -= S7.OnPlayerDied;
            PlayerEvent.ChangingRole -= S7.OnChangingRole;
        }
        private void LogInitialization()
        {
            Log.Info("=========XZPlugin=========");
            Log.Info($"   XZPlugin - {Version}");
            Log.Info($"   DongShanAPI - {Ver_API}");
            Log.Info($"   ServerCommandAdmin - {Ver_ServerCommandAdmin}");
            Log.Info("");
            Log.Info("   聊天系统已加载");
            Log.Info("   事件播报系统已加载");
            Log.Info("   玩家增强系统已加载");
            Log.Info("   游戏修复系统已加载");
            Log.Info("   称号系统已加载");
            Log.Info("");
            Log.Info("   SCPINFO已加载");
            Log.Info("   保安下班已加载");
            Log.Info("   硬币抽奖已加载");
            Log.Info("   更多人物已加载");
            Log.Info("   空投系统已加载");
            Log.Info("   等待界面已加载");
            Log.Info("   随机事件已加载");
            Log.Info("   无卡开门已加载");
            Log.Info("==============End================");
        }
        private void OnPlayerHurting(HurtingEventArgs ev)
        {
            if (ev.DamageType == DamageTypes.Scp207)
            {
                ev.IsAllowed = false;
            }
            if (ev.DamageType == DamageTypes.MicroHid)
            {
                ev.Amount = 250;
            }
        }
        private void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player == null)
            {
                return;
            }

            var FirstRole = ev.Player.Role;

            if (ev.NewRole.GetTeam() == Team.SCP)
            {
                Timing.CallDelayed(0.1f, () =>
                {
                    if (ev.Player.Role.GetTeam() == Team.SCP && ev.Player.Role != FirstRole)
                    {
                        ev.Player.SetRole(ev.NewRole, true);
                        RoleInitializationCriteria(ev.Player, ev.NewRole);
                    }
                });
            }
            else
            {
                Timing.CallDelayed(0.1f, () =>
                {
                    if (ev.Player.Role == ev.NewRole && ev.Player.Role != FirstRole)
                    {
                        RoleInitializationCriteria(ev.Player, ev.NewRole);
                    }
                });
            }
        }
        private void RoleInitializationCriteria(Player player, RoleType role) 
        {
            switch (role)
            {
                case RoleType.Scp106:
                    player.MaxHealth = 2500;
                    player.Health = 2500;
                    player.AddItem(ItemType.Coin);
                    player.AddItem(ItemType.Coin);
                    player.AddItem(ItemType.Coin);
                    player.AddItem(ItemType.Coin);
                    break;
                case RoleType.Scp096:
                    player.MaxHealth = 2000;
                    player.Health = 2000;
                    player.AddItem(ItemType.SCP207);
                    player.AddItem(ItemType.Coin);
                    player.AddItem(ItemType.Coin);
                    break;
                case RoleType.Scp173:
                    player.MaxHealth = 4000;
                    player.Health = 4000;
                    player.AddItem(ItemType.SCP207);
                    player.AddItem(ItemType.Coin);
                    break;
                case RoleType.Scp049:
                    player.MaxHealth = 3200;
                    player.Health = 3200;
                    player.AddItem(ItemType.Coin);
                    player.AddItem(ItemType.Coin);
                    player.AddItem(ItemType.Coin);
                    break;
                case RoleType.Scp0492:
                    player.MaxHealth = 1200;
                    player.Health = 1200;
                    player.AddItem(ItemType.SCP207);
                    player.AddItem(ItemType.SCP207);
                    player.AddItem(ItemType.SCP207);
                    break;
                case RoleType.Scp93953:
                    player.MaxHealth = 4000;
                    player.Health = 4000;
                    player.AddItem(ItemType.Coin);
                    player.AddItem(ItemType.Coin);
                    break;
                case RoleType.Scp93989:
                    player.MaxHealth = 4000;
                    player.Health = 4000;
                    player.AddItem(ItemType.Coin);
                    player.AddItem(ItemType.Coin);
                    break;
                case RoleType.NtfCadet:
                    player.MaxHealth = 120;
                    player.Health = 120;
                    player.AddItem(ItemType.GrenadeFrag);
                    player.AddItem(ItemType.Coin);
                    break;
                case RoleType.NtfCommander:
                    player.MaxHealth = 200;
                    player.Health = 200;
                    player.ClearInventory();
                    player.AddItem(ItemType.MicroHID);
                    player.AddItem(ItemType.GunE11SR);
                    player.AddItem(ItemType.GunUSP);
                    player.AddItem(ItemType.Medkit);
                    player.AddItem(ItemType.KeycardNTFCommander);
                    player.AddItem(ItemType.WeaponManagerTablet);
                    player.AddItem(ItemType.Disarmer);
                    player.AddItem(ItemType.Radio);
                    break;
                case RoleType.NtfLieutenant:
                    player.MaxHealth = 150;
                    player.Health = 150;
                    player.AddItem(ItemType.Adrenaline);
                    break;
                case RoleType.NtfScientist:
                    player.MaxHealth = 150;
                    player.Health = 150;
                    player.AddItem(ItemType.GrenadeFrag);
                    break;
                case RoleType.FacilityGuard:
                    player.MaxHealth = 120;
                    player.Health = 120;
                    player.ClearInventory();
                    player.AddItem(ItemType.GunProject90);
                    player.AddItem(ItemType.Coin);
                    player.AddItem(ItemType.KeycardGuard);
                    player.AddItem(ItemType.Radio);
                    player.AddItem(ItemType.WeaponManagerTablet);
                    player.AddItem(ItemType.Disarmer);
                    player.AddItem(ItemType.Medkit);
                    player.AddItem(ItemType.GrenadeFrag);
                    break;
                case RoleType.ChaosInsurgency:
                    player.MaxHealth = 150;
                    player.Health = 150;
                    player.ClearInventory();
                    player.AddItem(ItemType.GunLogicer);
                    player.AddItem(ItemType.Coin);
                    player.AddItem(ItemType.GrenadeFrag);
                    player.AddItem(ItemType.WeaponManagerTablet);
                    player.AddItem(ItemType.Disarmer);
                    player.AddItem(ItemType.KeycardChaosInsurgency);
                    player.AddItem(ItemType.Medkit);
                    player.AddItem(ItemType.Adrenaline);
                    break;
                case RoleType.ClassD:
                    player.AddItem(ItemType.KeycardJanitor);
                    player.AddItem(ItemType.Medkit);
                    player.AddItem(ItemType.Flashlight);
                    player.AddItem(ItemType.Coin);
                    break;
                case RoleType.Scientist:
                    player.AddItem(ItemType.Flashlight);
                    player.AddItem(ItemType.Coin);
                    break;
            }
        }//角色初始化标准
        private void CleanCorpses()
        {
            foreach (var ragdoll in UnityEngine.Object.FindObjectsOfType<Ragdoll>())
            {
                NetworkServer.Destroy(ragdoll.gameObject);
            }
            Map.Broadcast(4, "<size=40><color=yellow>[饿了喵]</color></size>\n<size=35><color=blue>现在本喵饿坏了! 要吃掉所有扔在地上的尸体咯~</color></size>", Broadcast.BroadcastFlags.Normal);
        }

        private void CleanItem()
        {
            foreach (var item in UnityEngine.Object.FindObjectsOfType<Pickup>())
            {
                NetworkServer.Destroy(item.gameObject);
            }
            Map.Broadcast(4, "<size=40><color=yellow>[饿了喵]</color></size>\n<size=35><color=blue>现在本喵饿坏了! 要吃掉所有扔在地上的东西咯~注意检查你的物品, 别让本喵误吃了! </color></size>", Broadcast.BroadcastFlags.Normal);
        }
        private void OnRoundStarted()
        {
            Map.Broadcast(4, $"<size=40><color=yellow>[回合消息]</color></size>\n<size=35><color=red>全体人员请注意! SCP收容失效! SCP收容失效! \n为确保您的生命安全, 请与设施安保会和! </color></size>");
            Map.Broadcast(4, "<size=35><color=yellow>欢迎游玩東山服务器!</color>\n<size=30><color=red>保安下班, 显示队友数量, 聊天系统, 硬币抽奖, 可乐不掉血, USP加强</color></size>\n<color=blue>加入我们的怀旧服Q群: 715253424</color></size>", Broadcast.BroadcastFlags.Normal);

            //物品清理
            foreach (float time in CleanCorpsesTime)
            {
                Timing.CallDelayed(time, CleanCorpses);
            }
            Timing.KillCoroutines(CleanCoroutines);
            CleanCoroutines = Timing.CallDelayed(CleanItemTime, CleanItem);

            //回合开始关友伤
            foreach (Player Ply in Player.List)
            {
                Ply.IsFriendlyFireEnabled = false;
            }
        }
        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            foreach (Player Ply in Player.List)
            {
                Ply.IsFriendlyFireEnabled = true;
            }
        }
        // {customRoleName}
        // {killerName}
        private void OnPlayerDying(DyingEventArgs ev)
        {
            if (ev.Target.Team == Team.SCP && ev.Target.Role != RoleType.Scp0492)
            {
                string killerName = ev.Killer != null ? ev.Killer.Nickname : "未知";
                string CN_Name = GetCN_Name(ev.Target.Role);

                string Hint = $"<align=left><size=25><color=yellow>[收容通知]</color><color=red><b>{CN_Name}</b></color>目前已被<b>{killerName}</b>收容</size></align>";

                foreach (Player p in Player.List)
                {
                    p.ARueiHint(400, Hint, 5);
                    p.SendConsoleMessage($"{CN_Name}被{killerName}收容了", "收容通知");
                }
            }

            string killermessage = $"<align=right><size=25>[个人消息] 你击杀了{ev.Target.Nickname} ({ev.Target.Role})! </size></align>";

            ev.Killer.ARueiHint(500, killermessage, 3);
            foreach (Player p in Player.List)
            {
                p.SendConsoleMessage($"{ev.Killer.Nickname}击杀了{ev.Target.Nickname}", "击杀消息");
            }
        }

        private string GetCN_Name(RoleType role)
        {
            switch (role)
            {
                case RoleType.Scp93989:
                    return "SCP-939-89";
                case RoleType.Scp173:
                    return "SCP-173";
                case RoleType.Scp049:
                    return "SCP-049";
                case RoleType.Scp0492:
                    return "SCP-049-2";
                case RoleType.Scp106:
                    return "SCP-106";
                case RoleType.Scp096:
                    return "SCP-096";
                case RoleType.Scp079:
                    return "SCP-079";
                case RoleType.Scp93953:
                    return "SCP-939-53";
                default:
                    return role.ToString();
            }
        }
        private void OnWarheadStarting(StartingEventArgs ev)
        {
            Map.Broadcast(2, "<size=40><color=yellow>[系统消息]</color></size>\n<size=35><color=red>警告! 核弹现已启动, 请所有人员撤离设施! 否则将被清除! </color></size>");
        }
        private void OnWarheadStopping(StoppingEventArgs ev)
        {
            Map.Broadcast(2, "<size=40><color=yellow>[系统消息]</color></size>\n<size=35><color=green>注意! 核弹现已终止, 系统现在开始重启! </color></size>");
        }
        private void OnWarheadDetonated()
        {
            Map.Broadcast(2, "<size=40><color=yellow>[系统消息]</color></size>\n<size=35><color=red>警告! 核弹现已被引爆!  辐射将透过任何物质持续对你造成伤害! </color></size>");

            foreach (Player player in Player.List)
            {
                StartRadiatione(player); //协程，启动！
            }
        }
        private void OnSawSCP096(EnragingEventArgs ev)
        {
            string Hint = "<b>扑通! SCP096被激怒了!</b>";
            ev.Player.RueiHint(300, Hint, 4);
        }
        private void OnDecontaminating(DecontaminatingEventArgs ev)
        {
            Map.Broadcast(5, "<size=40><color=yellow>[系统消息]</color></size>\n<size=35><color=red>全体人员请注意! \n轻收容现开始净化, 所有生物都将被清除! </color></size>");
        }
        private void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            if (ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency)
            {
                foreach (Player p in Player.List)
                {
                    p.RueiHint(300, "<b><color=green>孩子们刷混沌了</color></b>");
                }

                string CassieHINT = "all personnel . Chaos Insurgency forces have entered Gate A";
                Cassie.Message(CassieHINT);
            }


            else if (ev.NextKnownTeam == Respawning.SpawnableTeamType.NineTailedFox)
            {
                foreach (Player p in Player.List)
                {
                    p.RueiHint(300, "<b><color=blue>孩子们刷肘尾了</color></b>");
                }
            }
        }
        private IEnumerator<float> StartRadiatione(Player player)  //辐射伤害
        {
            while (player.IsAlive)
            {
                player.Hurt(1, DamageTypes.Nuke);
                yield return Timing.WaitForSeconds(1);
            }
        }
        private IEnumerator<float> SCPHealth() //SCP回血
        {
            while (true)
            {
                foreach (Player player in Player.List)
                {
                    if (!player.IsScp)
                        continue;

                    if (!LastPos.ContainsKey(player))
                    {
                        LastPos[player] = player.Position;
                        KeepPosTime[player] = 0f;
                    }
                    else
                    {
                        if (Vector3.Distance(player.Position, LastPos[player]) < 0.1f)
                        {
                            KeepPosTime[player] += 1f;

                            if (KeepPosTime[player] > Config.SCP_KeepPosTime && player.Health < player.MaxHealth)
                            {
                                player.Health += Config.SCP_Health;
                                player.Health = Mathf.Min(player.Health, player.MaxHealth);
                            }
                        }
                        else
                        {
                            LastPos[player] = player.Position;
                            KeepPosTime[player] = 0f;
                        }
                    }
                }
                yield return Timing.WaitForSeconds(1f);
            }
        }
    }
}