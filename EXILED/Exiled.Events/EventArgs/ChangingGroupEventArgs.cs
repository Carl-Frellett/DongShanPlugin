// -----------------------------------------------------------------------
// <copyright file="ChangingGroupEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs
{
    using Exiled.API.Features;
    using System;

    /// <summary>
    /// Contains all informations before a player changes his group.
    /// </summary>
    public class ChangingGroupEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangingGroupEventArgs"/> class.
        /// </summary>
        /// <param name="player"><inheritdoc cref="Player"/></param>
        /// <param name="newGroup"><inheritdoc cref="NewGroup"/></param>
        /// <param name="isAllowed"><inheritdoc cref="IsAllowed"/></param>
        public ChangingGroupEventArgs(Player player, UserGroup newGroup, bool isAllowed = true)
        {
            Player = player;
            NewGroup = newGroup;
            IsAllowed = isAllowed;
        }

        /// <summary>
        /// Gets the player who's changing his group.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets the player's new group.
        /// </summary>
        public UserGroup NewGroup { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event can be executed or not.
        /// </summary>
        public bool IsAllowed { get; set; }
    }
}
