using System;
using System.Linq;
using Exiled.API.Features;
using System.IO;

namespace DongShanAPI.SCA
{
    public static class AdminFileManager
    {
        private static readonly string adminFilePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SCP Secret Laboratory", "ServerAdmin.txt");

        public static bool IsAdmin(string username, string ipAddress)
        {
            if (!System.IO.File.Exists(adminFilePath))
            {
                Log.Error("未找到指定的ServerAdmin数据文件");
                return false;
            }

            var adminData = System.IO.File.ReadAllLines(adminFilePath);

            bool hasPermission = adminData.Any(line =>
                !string.IsNullOrWhiteSpace(line) &&
                (username.Equals("None", StringComparison.OrdinalIgnoreCase)
                    ? line.Contains(ipAddress)
                    : line.Contains(username) && line.Contains(ipAddress)));

            return hasPermission;
        }
    }
}
