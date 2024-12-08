using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DongShanAPI.SCA
{
    public static class OpenAdmin
    {
        private static readonly string adminFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SCP Secret Laboratory", "ServerAdmin.txt");

        public static List<(string Name, string IP)> LoadAdminList()
        {
            try
            {
                if (!File.Exists(adminFilePath))
                {
                    File.Create(adminFilePath).Close();
                }

                return File.ReadAllLines(adminFilePath)
                           .Select(line =>
                           {
                               var parts = line.Split(',');
                               return (parts[0], parts[1]);
                           })
                           .ToList();
            }
            catch (Exception)
            {
                return new List<(string Name, string IP)>();
            }
        }

        public static void AddAdmin(string playerName, string ipAddress)
        {
            var adminList = LoadAdminList();
            var adminEntry = (playerName, ipAddress);
            if (!adminList.Contains(adminEntry))
            {
                adminList.Add(adminEntry);
                File.WriteAllLines(adminFilePath, adminList.Select(a => $"{a.Name},{a.IP}"));
            }
        }

        public static bool IsAdmin(string ipAddress)
        {
            var adminList = LoadAdminList();
            return adminList.Any(admin => admin.IP == ipAddress);
        }

        public static void RemoveAdmin(string playerName, string ipAddress)
        {
            var adminList = LoadAdminList();
            var adminEntry = (playerName, ipAddress);
            adminList.Remove(adminEntry);
            File.WriteAllLines(adminFilePath, adminList.Select(a => $"{a.Name},{a.IP}"));
        }
    }
}
