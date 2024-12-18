using Nautilus.Assets;
using Nautilus.Handlers;
using System;
using System.Collections.Generic;

namespace Baselib.Handlers;

/// <summary>
/// Handles the registration of base piece's models.
/// </summary>
public static class BasePieceModelHandler
{
    private static readonly Dictionary<Base.Piece, PrefabInfo> _pieceToModelDictionary = new();

    /// <summary>
    /// A list containing all the registered models.
    /// </summary>
    public static Dictionary<PrefabInfo, Base.Piece> Models { get; } = new();

    /// <summary>
    /// Tries to register a model, and outputs a <seealso cref="Base.Piece"/> instance related to the model.
    /// </summary>
    /// <param name="model">The model to register.</param>
    /// <param name="resultPiece">The resulting <seealso cref="Base.Piece"/>.</param>
    /// <returns>True if the operation was successful, otherwise false.</returns>
    public static bool TryRegisterModel(PrefabInfo model, out Base.Piece resultPiece)
    {
        resultPiece = Base.Piece.Invalid;

        if (Models.ContainsKey(model))
        {
            InternalLogger.Warn("Tried registering a model that has already been registered! Skipping.");
            return false;
        }

        if (EnumHandler.TryAddEntry(model.ClassID, out EnumBuilder<Base.Piece> builder))
        {
            resultPiece = builder;
            Models.Add(model, builder);
            _pieceToModelDictionary.Add(builder, model);
            return true;
        }
        else
        {
            InternalLogger.Error($"Failed to register model {model.ClassID}.");
            return false;
        }
    }

    /// <summary>
    /// Tries to get a <seealso cref="Base.Piece"/> from a model reference.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <param name="piece">The resulting <seealso cref="Base.Piece"/>.</param>
    /// <returns>True if the operation was successful, otherwise false.</returns>
    public static bool TryGetPiece(PrefabInfo model, out Base.Piece piece)
    {
        if (Models.TryGetValue(model, out piece))
        {
            return true;
        }

        InternalLogger.Error($"Failed to get piece from model: {model.ClassID}. Is the model registered?");
        return false;
    }

    /// <summary>
    /// Tries to get a model from a <seealso cref="Base.Piece"/>.
    /// </summary>
    /// <param name="piece">The piece.</param>
    /// <param name="model">The resulting model.</param>
    /// <returns>True if the operation was successful, otherwise false.</returns>
    public static bool TryGetModel(Base.Piece piece, out PrefabInfo model)
    {
        if (_pieceToModelDictionary.TryGetValue(piece, out model))
        {
            return true;
        }

        InternalLogger.Error($"Failed to get model from piece: {piece}. Is the model registered?");
        return false;
    }
}
