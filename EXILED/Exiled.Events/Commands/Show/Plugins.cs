// -----------------------------------------------------------------------
// <copyright file="Plugins.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Commands
{
    using CommandSystem;
    using NorthwoodLib.Pools;
    using System;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The command to show all plugins.
    /// </summary>
    public sealed class Plugins : ICommand
    {
        /// <inheritdoc/>
        public string Command { get; } = "plugins";

        /// <inheritdoc/>
        public string[] Aliases { get; } = { "sp", "showplugins" };

        /// <inheritdoc/>
        public string Description { get; } = "Gets all plugins, names, authors and versions";

        /// <inheritdoc/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var sb = StringBuilderPool.Shared.Rent();

            // Append a new line to start the response on a new line
            sb.AppendLine();

            var plugins = Exiled.Loader.Loader.Plugins;

            // Append two new lines before the list
            sb.Append("Total number of plugins: ").Append(plugins.Count).AppendLine().AppendLine();

            StringBuilder AppendNewRow() => sb.AppendLine().Append("\t");

            for (var z = 0; z < plugins.Count; z++)
            {
                var plugin = plugins.ElementAt(z);

                sb.Append(string.IsNullOrEmpty(plugin.Name) ? "(Unknown)" : plugin.Name).Append(":");

                AppendNewRow().Append("- Author: ").Append(plugin.Author);
                AppendNewRow().Append("- Version: ").Append(plugin.Version);
                AppendNewRow().Append("- Required Exiled Version: ").Append(plugin.RequiredExiledVersion);
                AppendNewRow().Append("- Prefix: ").Append(plugin.Prefix);
                AppendNewRow().Append("- Priority: ").Append(plugin.Priority);

                // Finalize a plugin row if it's not the end
                if (z + 1 != plugins.Count)
                    sb.AppendLine();
            }

            response = sb.ToString();
            StringBuilderPool.Shared.Return(sb);
            return true;
        }
    }
}
