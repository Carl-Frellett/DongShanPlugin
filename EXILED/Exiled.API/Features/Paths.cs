// -----------------------------------------------------------------------
// <copyright file="Paths.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Features
{
    using System;
    using System.IO;

    /// <summary>
    /// A set of useful paths.
    /// </summary>
    public static class Paths
    {
        static Paths() => Reload();

        /// <summary>
        /// Gets AppData path.
        /// </summary>
        public static string AppData { get; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        /// <summary>
        /// Gets managed assemblies directory path.
        /// </summary>
        public static string ManagedAssemblies { get; } = Path.Combine(Path.Combine(Environment.CurrentDirectory, "SCPSL_Data"), "Managed");

        /// <summary>
        /// Gets or sets exiled directory path.
        /// </summary>
        public static string Exiled { get; set; }

        /// <summary>
        /// Gets or sets plugins path.
        /// </summary>
        public static string Plugins { get; set; }

        /// <summary>
        /// Gets or sets Dependencies directory path.
        /// </summary>
        public static string Dependencies { get; set; }

        /// <summary>
        /// Gets or sets configs path.
        /// </summary>
        public static string Configs { get; set; }

        /// <summary>
        /// Gets or sets configs path.
        /// </summary>
        public static string Config { get; set; }

        /// <summary>
        /// Gets or sets logs path.
        /// </summary>
        public static string Log { get; set; }

        /// <summary>
        /// Gets or sets translations path.
        /// </summary>
        public static string Translations { get; set; }

        /// <summary>
        /// Reloads all paths.
        /// </summary>
        /// <param name="rootDirectoryName">The new root directory name.</param>
        public static void Reload(string rootDirectoryName = "EXILED")
        {
            Exiled = Path.Combine(AppData, rootDirectoryName);
            Plugins = Path.Combine(Exiled, "Plugins");
            Dependencies = Path.Combine(Plugins, "dependencies");
            Configs = Path.Combine(Exiled, "Configs");
            Config = Path.Combine(Configs, $"{Server.Port}-config.yml");
            Log = Path.Combine(Exiled, $"{Server.Port}-RemoteAdminLog.txt");
            Translations = Path.Combine(Configs, $"{Server.Port}-translations.yml");
        }
    }
}
