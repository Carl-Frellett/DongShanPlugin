// -----------------------------------------------------------------------
// <copyright file="ChangingIntercomMuteStatusEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs
{
    using Exiled.API.Features;
    using System;

    /// <summary>
    /// Contains all informations before a player's intercom mute status is changed.
    /// </summary>
    public class ChangingIntercomMuteStatusEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangingIntercomMuteStatusEventArgs"/> class.
        /// </summary>
        /// <param name="player"><inheritdoc cref="Player"/></param>
        /// <param name="isMuted"><inheritdoc cref="IsMuted"/></param>
        /// <param name="isAllowed"><inheritdoc cref="IsAllowed"/></param>
        public ChangingIntercomMuteStatusEventArgs(Player player, bool isMuted, bool isAllowed = true)
        {
            Player = player;
            IsMuted = isMuted;
            IsAllowed = isAllowed;
        }

        /// <summary>
        /// Gets the player who's being intercom muted/unmuted.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the event can be executed or not.
        /// </summary>
        public bool IsAllowed { get; set; }

        /// <summary>
        /// Gets a value indicating whether the player is being intercom muted or unmuted.
        /// </summary>
        public bool IsMuted { get; }
    }
}
