// -----------------------------------------------------------------------
// <copyright file="Show.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Commands
{
    using CommandSystem;
    using System;

    /// <summary>
    /// The command to show all plugins.
    /// </summary>
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public sealed class Show : ParentCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Show"/> class.
        /// </summary>
        public Show() => LoadGeneratedCommands();

        /// <inheritdoc/>
        public override string Command { get; } = "show";

        /// <inheritdoc/>
        public override string[] Aliases { get; } = { };

        /// <inheritdoc/>
        public override string Description { get; } = string.Empty;

        /// <inheritdoc/>
        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new Plugins());
        }

        /// <inheritdoc/>
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "Please, specify a valid subcommand! Available: plugins";
            return false;
        }
    }
}
