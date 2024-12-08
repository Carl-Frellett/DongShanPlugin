using DongShanAPI.Hint;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace XZPlugin
{
    class SEe
    {
        public Random rng = new Random();
        public CoroutineHandle Coroutine;

        string Hint1 = "<align=left><size=25><color=blue>[随机事件-为人民而死]</color> 因为一名DD的死亡, 其余所有DD都将获得武器!</size></align>";
        string Hint2 = "<align=left><size=25><color=blue>[随机事件-小人国]</color> 所有玩家都变小了!</size></align>";
        string Hint3 = "<align=left><size=25><color=blue>[随机事件-超级DD]</color> 有一名DD获得了加强!</size></align>";
        string Hint4_96 = "<align=left><size=25><color=blue>[随机事件-96变黑了]</color> 96变黑了？!</size></align>";
        string Hint4_106 = "<align=left><size=25><color=blue>[随机事件-老头洗澡了]</color> 洗香香! 洗白白!</size></align>";
        string Hint5 = "<align=left><size=25><color=blue>[随机事件-真就花生了!]</color> 花生被缩小了!</size></align>";
        string Hint6 = "<align=left><size=25><color=blue>[随机事件-谁家的狗]</color> 939严重缩水!</size></align>";
        public void OnRoundStarted()
        {
            Coroutine = Timing.RunCoroutine(SomeEvents());
        }

        public void OnPlayerDied(DiedEventArgs ev)
        {
            if (ev.Target.Role == RoleType.ClassD && rng.Next(100) < 10)
            {
                foreach (var player in Player.Get(RoleType.ClassD))
                {
                    player.AddItem(ItemType.GunCOM15);
                }

                foreach (Player p in Player.List)
                {
                    p.RueiHint(400, Hint1, 5);
                }
                global::ServerConsole.AddLog("[XZPlugin] 事件-为人民而死 ", ConsoleColor.DarkGreen);
            }
        }
        private IEnumerator<float> SomeEvents()
        {
            yield return Timing.WaitForSeconds(120);
            SupperClassD();

            yield return Timing.WaitForSeconds(120);
            Samll939();

            yield return Timing.WaitForSeconds(60);
            SwapSCP();

            yield return Timing.WaitForSeconds(60);
            Samll173();

            yield return Timing.WaitForSeconds(60);
            SetAllSize();
        }
        private void SetAllSize()
        {
            if (Player.List.Count() >= 13)
            {
                if (rng.Next(100) < 1)
                {
                    foreach (Player player in Player.List)
                    {
                        player.Scale = new Vector3(0.7f, 0.7f, 0.7f);
                    }

                    foreach (Player p in Player.List)
                    {
                        p.RueiHint(400, Hint2, 3);
                    }
                    global::ServerConsole.AddLog("[XZPlugin] 事件-小人国", ConsoleColor.DarkGreen);
                }
            }
        }

        private void SupperClassD()
        {
            var ClassD = Player.Get(RoleType.ClassD).ToList();
            if (ClassD.Count > 0)
            {
                var SupperD = ClassD[rng.Next(ClassD.Count)];
                SupperD.AddItem(ItemType.GunUSP);
                SupperD.AddItem(ItemType.Medkit);
                SupperD.EnableEffect<CustomPlayerEffects.Scp207>(990f,true);
                SupperD.EnableEffect<CustomPlayerEffects.Scp207>(990f, true);


                foreach (Player p in Player.List)
                {
                    p.RueiHint(400, Hint3, 3);
                }
                global::ServerConsole.AddLog("[XZPlugin] 事件-超级DD", ConsoleColor.DarkGreen);
            }
        }

        private void SwapSCP()
        {
            if (rng.Next(100) < 30)
            {
                var _96 = Player.Get(RoleType.Scp096).FirstOrDefault();
                var _106 = Player.Get(RoleType.Scp106).FirstOrDefault();

                if (_96 != null)
                {
                    _96.SetRole(RoleType.Scp106);

                    foreach (Player p in Player.List)
                    {
                        p.RueiHint(400, Hint4_96, 5);
                    }
                    global::ServerConsole.AddLog("[XZPlugin] 事件-96变黑了", ConsoleColor.DarkGreen);
                }
                else if (_106 != null)
                {
                    _106.SetRole(RoleType.Scp096);

                    foreach (Player p in Player.List)
                    {
                        p.RueiHint(400, Hint4_106, 5);
                    }
                    global::ServerConsole.AddLog("[XZPlugin] 事件-老头洗澡了", ConsoleColor.DarkGreen);
                }
            }
        }

        private void Samll173()
        {
            if (rng.Next(100) < 30)
            {
                var _173 = Player.Get(RoleType.Scp173).FirstOrDefault();
                if (_173 != null)
                {
                    _173.Scale = new Vector3(0.7f, 0.7f, 0.7f);
                    _173.Health = 1000;
                    _173.MaxHealth = 1000;

                    foreach (Player p in Player.List)
                    {
                        p.RueiHint(400, Hint5, 5);
                    }
                    global::ServerConsole.AddLog("[XZPlugin] 事件-真就花生了", ConsoleColor.DarkGreen);
                }
            }
        }

        private void Samll939()
        {
            if (rng.Next(100) < 70)
            {
                var _939 = Player.Get(RoleType.Scp93953).Concat(Player.Get(RoleType.Scp93989)).ToList();
                if (_939.Count > 0)
                {
                    var SCP939 = _939[rng.Next(_939.Count)];
                    SCP939.Scale = new Vector3(0.7f, 0.7f, 0.7f);

                    foreach (Player p in Player.List)
                    {
                        p.RueiHint(400, Hint6, 5);
                    }
                    global::ServerConsole.AddLog("[XZPlugin] 事件-谁家的狗", ConsoleColor.DarkGreen);
                }
            }
        }
    }
}
