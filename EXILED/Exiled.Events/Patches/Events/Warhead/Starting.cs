// -----------------------------------------------------------------------
// <copyright file="Starting.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Patches.Events.Warhead
{
#pragma warning disable SA1118
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using HarmonyLib;
    using NorthwoodLib.Pools;
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using UnityEngine;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patch the <see cref="PlayerInteract.CallCmdDetonateWarhead"/>.
    /// Adds the <see cref="Handlers.Warhead.Starting"/> event.
    /// </summary>
    [HarmonyPatch(typeof(PlayerInteract), nameof(PlayerInteract.CallCmdDetonateWarhead))]
    internal static class Starting
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            // The index offset.
            const int offset = 0;

            // Search for the last "ldsfld".
            var index = newInstructions.FindLastIndex(instruction => instruction.opcode == OpCodes.Ldsfld) + offset;

            // Get the starting labels and remove all of them from the original instruction.
            var startingLabels = newInstructions[index].labels;

            // Get the return label.
            var returnLabel = newInstructions[index - 1].labels[0];

            // Remove "ldsfld AlphaWarheadController::Host" & "AlphawarheadController.Host.StartDetonation()".
            newInstructions.RemoveRange(index, 2);

            // if (!Warhead.CanBeStarted)
            //   return;
            //
            // var ev = new StartingEventArgs(Player.Get(this.gameObject), true);
            //
            // Handlers.Warhead.OnStarting(ev);
            //
            // if (!ev.IsAllowed)
            //   return;
            //
            // AlphaWarheadController.Host.doorsOpen = false;
            // AlphaWarheadController.Host.NetworkinProgress = true;
            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(Warhead), nameof(Warhead.CanBeStarted))),
                new CodeInstruction(OpCodes.Brfalse_S, returnLabel),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Component), nameof(Component.gameObject))),
                new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(GameObject) })),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(StartingEventArgs))[0]),
                new CodeInstruction(OpCodes.Dup),
                new CodeInstruction(OpCodes.Call, Method(typeof(Handlers.Warhead), nameof(Handlers.Warhead.OnStarting))),
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(StartingEventArgs), nameof(StartingEventArgs.IsAllowed))),
                new CodeInstruction(OpCodes.Brfalse_S, returnLabel),
                new CodeInstruction(OpCodes.Ldsfld, Field(typeof(AlphaWarheadController), nameof(AlphaWarheadController.Host))),
                new CodeInstruction(OpCodes.Dup),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Stfld, Field(typeof(AlphaWarheadController), nameof(AlphaWarheadController.doorsOpen))),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Call, PropertySetter(typeof(AlphaWarheadController), nameof(AlphaWarheadController.NetworkinProgress))),
            });

            // Add the starting labels to the first injected instruction.
            newInstructions[index].WithLabels(startingLabels);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}
