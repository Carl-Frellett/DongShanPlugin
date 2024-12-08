// -----------------------------------------------------------------------
// <copyright file="UsedMedicalItemEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs
{
    using Exiled.API.Features;
    using System;

    /// <summary>
    /// Contains all informations after a player uses a medical item on himself.
    /// </summary>
    public class UsedMedicalItemEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsedMedicalItemEventArgs"/> class.
        /// </summary>
        /// <param name="player"><inheritdoc cref="Player"/></param>
        /// <param name="item"><inheritdoc cref="Item"/></param>
        public UsedMedicalItemEventArgs(Player player, ItemType item)
        {
            Player = player;
            Item = item;
        }

        /// <summary>
        /// Gets the player who used the medical item.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the medical item that the player consumed.
        /// </summary>
        public ItemType Item { get; }
    }
}
