﻿// -----------------------------------------------------------------------
// <copyright file="RecontainedEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs
{
    using Exiled.API.Features;
    using System;

    /// <summary>
    /// Contains informations after SCP-079 recontainming.
    /// </summary>
    public class RecontainedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecontainedEventArgs"/> class.
        /// </summary>
        /// <param name="target"><inheritdoc cref="Target"/></param>
        public RecontainedEventArgs(Player target)
        {
            Target = target;
        }

        /// <summary>
        /// Gets the player that was SCP-079.
        /// </summary>
        public Player Target { get; }
    }
}
