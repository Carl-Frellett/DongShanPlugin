// -----------------------------------------------------------------------
// <copyright file="Containing.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Patches.Events.Scp106
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
    /// Patches <see cref="PlayerInteract.CallCmdContain106"/>.
    /// Adds the <see cref="Handlers.Scp106.Containing"/> event.
    /// </summary>
    [HarmonyPatch(typeof(PlayerInteract), nameof(PlayerInteract.CallCmdContain106))]
    internal static class Containing
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            // The index offset.
            const int offset = 1;

            // Search for the last "bne.un.s" and add 1 to get the index of the last "ldloca.s".
            var index = newInstructions.FindLastIndex(instruction => instruction.opcode == OpCodes.Bne_Un_S) + offset;

            // Get the return label.
            var returnLabel = newInstructions[index - 1].operand;

            // var ev = new ContainingEventArgs(Player.Get(keyValuePair.Key), true);
            //
            // Handlers.Scp106.OnContaining(ev);
            //
            // if (!ev.IsAllowed)
            //   return;
            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Ldloca_S, 2),
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(KeyValuePair<GameObject, ReferenceHub>), nameof(KeyValuePair<GameObject, ReferenceHub>.Key))),
                new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(GameObject) })),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(ContainingEventArgs))[0]),
                new CodeInstruction(OpCodes.Dup),
                new CodeInstruction(OpCodes.Call, Method(typeof(Handlers.Scp106), nameof(Handlers.Scp106.OnContaining))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(ContainingEventArgs), nameof(ContainingEventArgs.IsAllowed))),
                new CodeInstruction(OpCodes.Brfalse_S, returnLabel),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}
