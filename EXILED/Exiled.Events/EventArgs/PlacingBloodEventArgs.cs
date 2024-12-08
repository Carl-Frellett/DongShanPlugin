// -----------------------------------------------------------------------
// <copyright file="PlacingBloodEventArgs.cs" company="Exiled Team">
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
    /// Contains all informations before a player places blood.
    /// </summary>
    public class PlacingBloodEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlacingBloodEventArgs"/> class.
        /// </summary>
        /// <param name="player"><inheritdoc cref="Player"/></param>
        /// <param name="position"><inheritdoc cref="Position"/></param>
        /// <param name="type"><inheritdoc cref="Type"/></param>
        /// <param name="multiplier"><inheritdoc cref="Multiplier"/></param>
        /// <param name="isAllowed"><inheritdoc cref="IsAllowed"/></param>
        public PlacingBloodEventArgs(Player player, Vector3 position, int type, float multiplier, bool isAllowed = true)
        {
            Player = player;
            Position = position;
            Type = type;
            Multiplier = multiplier;
            IsAllowed = isAllowed;
        }

        /// <summary>
        /// Gets the player who's placing the blood.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets the blood placing position.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the blood type.
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the blood multiplier.
        /// </summary>
        public float Multiplier { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event can be executed or not.
        /// </summary>
        public bool IsAllowed { get; set; }
    }
}
