// -----------------------------------------------------------------------
// <copyright file="EnteringPocketDimensionEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs
{
    using Exiled.API.Features;
    using System;
    using UnityEngine;

    /// <summary>
    /// Contains all informations before a player enters the pocket dimension.
    /// </summary>
    public class EnteringPocketDimensionEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnteringPocketDimensionEventArgs"/> class.
        /// </summary>
        /// <param name="player"><inheritdoc cref="Player"/></param>
        /// <param name="position"><inheritdoc cref="Position"/></param>
        /// <param name="isAllowed"><inheritdoc cref="IsAllowed"/></param>
        public EnteringPocketDimensionEventArgs(Player player, Vector3 position, bool isAllowed = true)
        {
            Player = player;
            Position = position;
            IsAllowed = isAllowed;
        }

        /// <summary>
        /// Gets the player who's entering the pocket dimension.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets the pocket dimension position.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event can be executed or not.
        /// </summary>
        public bool IsAllowed { get; set; }
    }
}
