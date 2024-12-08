// -----------------------------------------------------------------------
// <copyright file="StoppingEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs
{
    using Exiled.API.Features;
    using System;

    /// <summary>
    /// Contains all informations before stopping the warhead.
    /// </summary>
    public class StoppingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoppingEventArgs"/> class.
        /// </summary>
        /// <param name="player"><inheritdoc cref="Player"/></param>
        /// <param name="isAllowed"><inheritdoc cref="IsAllowed"/></param>
        public StoppingEventArgs(Player player, bool isAllowed = true)
        {
            Player = player ?? Server.Host;
            IsAllowed = isAllowed;
        }

        /// <summary>
        /// Gets the player who's going to stop the warhead.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the event can be executed or not.
        /// </summary>
        public bool IsAllowed { get; set; }
    }
}
