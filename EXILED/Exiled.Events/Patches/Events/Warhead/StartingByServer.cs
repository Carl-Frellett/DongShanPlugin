// -----------------------------------------------------------------------
// <copyright file="StartingByServer.cs" company="Exiled Team">
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
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patch the <see cref="AlphaWarheadController.StartDetonation"/>.
    /// Adds the <see cref="Handlers.Warhead.Starting"/> event.
    /// </summary>
    [HarmonyPatch(typeof(AlphaWarheadController), nameof(AlphaWarheadController.StartDetonation))]
    internal static class StartingByServer
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            const int offset = 0;

            // Search for the last "ldarg.0".
            var index = newInstructions.FindLastIndex(instruction => instruction.opcode == OpCodes.Ldarg_0) + offset;

            // Get the count to find the previous index
            var oldCount = newInstructions.Count;

            // Get the return label from the last instruction.
            var returnLabel = newInstructions[index - 1].operand;

            // var ev = new StartingEventArgs(Server.Host, true);
            //
            // Handlers.Warhead.OnStarting(ev);
            //
            // if (!ev.IsAllowed)
            //   return;
            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(Server), nameof(Server.Host))),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(StartingEventArgs))[0]),
                new CodeInstruction(OpCodes.Dup),
                new CodeInstruction(OpCodes.Call, Method(typeof(Handlers.Warhead), nameof(Handlers.Warhead.OnStarting))),
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(StartingEventArgs), nameof(StartingEventArgs.IsAllowed))),
                new CodeInstruction(OpCodes.Brfalse_S, returnLabel),
            });

            // Add the starting labels to the first injected instruction.
            // Calculate the difference and get the valid index - is better and easy than using a list
            newInstructions[index].MoveLabelsFrom(newInstructions[newInstructions.Count - oldCount + index]);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}
