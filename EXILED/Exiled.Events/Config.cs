// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events
{
    using Exiled.API.Interfaces;
    using System.ComponentModel;

    /// <inheritdoc cref="IConfig"/>
    public sealed class Config : IConfig
    {
        /// <inheritdoc/>
        [Description("Indicates whether the plugin is enabled or not")]
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc/>
        [Description("Indicates whether the plugin is Debug")]
        public bool DEBUG { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether SCP-173 can be blocked or not by the tutorial.
        /// </summary>
        [Description("Indicates whether SCP-173 can be blocked or not by the tutorial")]
        public bool CanTutorialBlockScp173 { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether SCP-096 can be triggered or not by the tutorial.
        /// </summary>
        [Description("Indicates whether SCP-096 can be triggered or not by the tutorial")]
        public bool CanTutorialTriggerScp096 { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the name tracking is enabled or not.
        /// </summary>
        [Description("Indicates whether the name tracking is enabled or not")]
        public bool IsNameTrackingEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the inventory should be dropped before being set as spectator, through commands or plugins.
        /// </summary>
        [Description("Indicates whether the inventory should be dropped before being set as spectator, through commands or plugins")]
        public bool ShouldDropInventory { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the blood can be spawned or not.
        /// </summary>
        [Description("Indicates whether the blood can be spawned or not")]
        public bool CanSpawnBlood { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether configs has to be reloaded every time a round restarts.
        /// </summary>
        public bool ShouldReloadConfigsAtRoundRestart { get; set; }

        /// <summary>
        /// Gets a value indicating whether bans should be logged or not.
        /// </summary>
        [Description("Indicates whether bans should be logged or not")]
        public bool ShouldLogBans { get; private set; } = true;

        /// <summary>
        /// Gets or sets a value indicating the max shield amount for Scp096.
        /// </summary>
        [Description("The maximum shield amount for Scp096")]
        public int Scp096MaxShieldAmount { get; set; } = 350;
    }
}
