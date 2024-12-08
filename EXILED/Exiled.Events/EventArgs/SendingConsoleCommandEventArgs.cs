// -----------------------------------------------------------------------
// <copyright file="SendingConsoleCommandEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs
{
    using Exiled.API.Features;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Contains all informations before sending a console message.
    /// </summary>
    public class SendingConsoleCommandEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SendingConsoleCommandEventArgs"/> class.
        /// </summary>
        /// <param name="player"><inheritdoc cref="Player"/></param>
        /// <param name="name"><inheritdoc cref="Name"/></param>
        /// <param name="arguments"><inheritdoc cref="Arguments"/></param>
        /// <param name="isEncrypted"><inheritdoc cref="IsEncrypted"/></param>
        /// <param name="returnMessage"><inheritdoc cref="ReturnMessage"/></param>
        /// <param name="color"><inheritdoc cref="Color"/></param>
        /// <param name="isAllowed"><inheritdoc cref="Allow"/></param>
        public SendingConsoleCommandEventArgs(
            Player player,
            string name,
            List<string> arguments,
            bool isEncrypted,
            string returnMessage = "",
            string color = "white",
            bool isAllowed = true)
        {
            Player = player;
            Name = name;
            Arguments = arguments;
            IsEncrypted = isEncrypted;
            ReturnMessage = returnMessage;
            Color = color;
            IsAllowed = isAllowed;
        }

        /// <summary>
        /// Gets the player who's sending the command.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the command arguments.
        /// </summary>
        public List<string> Arguments { get; }

        /// <summary>
        /// Gets a value indicating whether the command is encrypted or not.
        /// </summary>
        public bool IsEncrypted { get; }

        /// <summary>
        /// Gets or sets the return message, that will be shown to the user in the console.
        /// </summary>
        public string ReturnMessage { get; set; }

        /// <summary>
        /// Gets or sets the color of the return message.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event can be executed or not.
        /// </summary>
        public bool Allow
        {
            get => IsAllowed;
            set => IsAllowed = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the event can be executed or not.
        /// </summary>
        public bool IsAllowed { get; set; }
    }
}
