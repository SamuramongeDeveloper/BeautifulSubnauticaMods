using Baselib.Handlers;
using HarmonyLib;
using Nautilus.Handlers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UWE;

namespace Baselib.Patches;

[HarmonyPatch(typeof(Base))]
public static class BasePatches
{
    [HarmonyPatch(nameof(Base.InitializeAsync))]
    [HarmonyPostfix]
    private static IEnumerator InitializeAsyncTranspiler(IEnumerator result)
    {
        while (result.MoveNext())
            yield return result.Current;

        yield return OverrideBasePieces();
        yield break;
    }

    private static IEnumerator OverrideBasePieces()
    {
        // Copy Base.pieces to basePiecesOverride array.
        var basePiecesOverride = new Base.PieceDef[Base.pieces.Length + BasePieceModelHandler.Models.Count + 1];
        Array.Copy(Base.pieces, basePiecesOverride, Base.pieces.Length);


        var customModelsToAdd = new List<Base.PieceDef>(BasePieceModelHandler.Models.Count);
        foreach (var model in BasePieceModelHandler.Models) // Add the new models to customModelsToAdd list.
        {
            var info = model.Key;

            var request = PrefabDatabase.GetPrefabAsync(info.ClassID);
            yield return request;

            if (request.TryGetPrefab(out GameObject prefab))
            {
                var pieceDef = new Base.PieceDef(prefab, new Int3(1), Quaternion.identity); // TODO: Add custom rotation and cellSize (extraCells).
                customModelsToAdd.Add(pieceDef);

                InternalLogger.Info($"Successfuly found prefab for model: {info.ClassID}");
            }
            else
            {
                InternalLogger.Error($"Failed to get prefab for model: {info.ClassID}.");
            }
        }

        // Copy customModelsToAdd elements into basePiecesOverride starting at the last vanilla last piece plus one.
        var customModelsToAddArrayConversion = customModelsToAdd.ToArray();
        Array.Copy(customModelsToAddArrayConversion, 0, basePiecesOverride, Base.pieces.Length + 1, customModelsToAddArrayConversion.Length);
        Base.pieces = basePiecesOverride; // Override Base.Pieces.
    }

    [HarmonyPatch(nameof(Base.BuildGeometryForCell))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> BuildGeometryForCellTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        var codeMatcher = new CodeMatcher(instructions, generator)
            .CreateLabelAt(30, out Label runSwitchStatementLabel)
            .MatchForward(true,
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(Base), nameof(Base.cells))),
                new CodeMatch(OpCodes.Ldloc_0),
                new CodeMatch(OpCodes.Ldelem_U1),
                new CodeMatch(OpCodes.Stloc_2))
            .Advance(1)
            .Insert(
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Ldloc_2),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(BasePatches), nameof(TryBuildCustomGeometryForCell))),
                new CodeInstruction(OpCodes.Brfalse_S, runSwitchStatementLabel),
                new CodeInstruction(OpCodes.Ret))
            .InstructionEnumeration();
        return codeMatcher;
    }

    private static bool TryBuildCustomGeometryForCell(Base instance, Int3 cell, Base.CellType cellType)
    {
        if (CustomCellTypesHandler.Cells.Contains(cellType))
        {
            // TODO, use another method to get the piece.
            CustomCellTypesHandler.TryGetName(cellType, out var name);
            EnumHandler.TryGetValue(name+"baseshape", out Base.Piece piece);
            InternalLogger.Info($"BuildCustomGeometryForCell called successfully, cellTypeName: {name}(baseshape), piece: {piece}.");
            instance.SpawnPiece(piece, cell);
            return true;
        }
        return false;
    }
}
