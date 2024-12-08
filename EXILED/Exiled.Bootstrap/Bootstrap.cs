// -----------------------------------------------------------------------
// <copyright file="Bootstrap.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Boostrap
{
#pragma warning disable SA1118
    using NorthwoodLib;
    using System;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// The assembly loader class for Exiled.
    /// </summary>
    public sealed class Bootstrap
    {
        /// <summary>
        /// Gets a value indicating whether exiled has already been loaded or not.
        /// </summary>
        public static bool IsLoaded { get; private set; }

        /// <summary>
        /// Internally called loading method.
        /// </summary>
        public static void Load()
        {
            if (IsLoaded == true)
            {
                ServerConsole.AddLog("[Exiled.Bootstrap] Exiled has already been loaded!", ConsoleColor.DarkRed);
                return;
            }
            else
            {
                try
                {
                    ServerConsole.AddLog("[Exiled.Bootstrap] Exiled is loading...", ConsoleColor.DarkRed);

                    string rootPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "EXILED");
                    string dependenciesPath = Path.Combine(rootPath, "Plugins", "dependencies");

                    if (Environment.CurrentDirectory.Contains("testing", StringComparison.OrdinalIgnoreCase))
                        rootPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "EXILED-Testing");

                    if (!Directory.Exists(rootPath))
                        Directory.CreateDirectory(rootPath);

                    if (File.Exists(Path.Combine(rootPath, "Exiled.Loader.dll")))
                    {
                        if (File.Exists(Path.Combine(dependenciesPath, "Exiled.API.dll")))
                        {
                            if (File.Exists(Path.Combine(dependenciesPath, "YamlDotNet.dll")))
                            {
                                Assembly.Load(File.ReadAllBytes(Path.Combine(rootPath, "Exiled.Loader.dll")))
                                    .GetType("Exiled.Loader.Loader")
                                    .GetMethod("Run")
                                    ?.Invoke(
                                        null,
                                        new object[]
                                        {
                                        new Assembly[]
                                        {
                                            Assembly.Load(File.ReadAllBytes(Path.Combine(dependenciesPath, "Exiled.API.dll"))),
                                            Assembly.Load(File.ReadAllBytes(Path.Combine(dependenciesPath, "YamlDotNet.dll"))),
                                        },
                                        });

                                IsLoaded = true;
                            }
                            else
                            {
                                ServerConsole.AddLog($"[Exiled.Bootstrap] YamlDotNet.dll was not found, Exiled won't be loaded!", ConsoleColor.DarkRed);
                            }
                        }
                        else
                        {
                            ServerConsole.AddLog($"[Exiled.Bootstrap] Exiled.API.dll was not found, Exiled won't be loaded!", ConsoleColor.DarkRed);
                        }
                    }
                    else
                    {
                        ServerConsole.AddLog($"[Exiled.Bootstrap] Exiled.Loader.dll was not found, Exiled won't be loaded!", ConsoleColor.DarkRed);
                    }
                }
                catch (Exception exception)
                {
                    ServerConsole.AddLog($"[Exiled.Bootstrap] Exiled loading error: {exception}", ConsoleColor.DarkRed);
                }
            }
        }
    }
}
