using Nautilus.Handlers;
using System.Collections.Generic;

namespace Baselib.Handlers;

/// <summary>
/// Handles the creation of new <seealso cref="Base.Piece"/>s.
/// </summary>
public static class CustomPiecesHandler
{
    private static readonly Dictionary<Base.Piece, string> _pieceToString = new();

    /// <summary>
    /// A list holding all the custom <seealso cref="Base.Piece"/>s.
    /// </summary>
    public static List<Base.Piece> Pieces { get; } = new();

    /// <summary>
    /// Tries to add a custom <seealso cref="Base.Piece"/>.
    /// </summary>
    /// <param name="name">The name of the custom <seealso cref="Base.Piece"/>.</param>
    /// <param name="result">The resulting <seealso cref="Base.Piece"/> if added correctly.</param>
    /// <returns>True if added correctly, otherwise false.</returns>
    public static bool TryAdd(string name, out Base.Piece result)
    {
        if (EnumHandler.TryAddEntry(name, out EnumBuilder<Base.Piece> pieceBuilder))
        {
            _pieceToString.Add(pieceBuilder, name);
            Pieces.Add(pieceBuilder);
            result = pieceBuilder;
            return true;
        }
        result = Base.Piece.Invalid;
        return false;
    }

    /// <summary>
    /// Tries to get a custom <seealso cref="Base.Piece"/> from a name.
    /// </summary>
    /// <param name="name">The name of the <seealso cref="Base.Piece"/>.</param>
    /// <param name="result">The resulting <seealso cref="Base.Piece"/>.</param>
    /// <returns>True if the <seealso cref="Base.Piece"/> exists, otherwise false.</returns>
    public static bool TryGet(string name, out Base.Piece result)
    {
        if (EnumHandler.TryGetValue(name, out result))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Tries to get the name of a custom <seealso cref="Base.Piece"/>.
    /// </summary>
    /// <param name="pieceType">The <seealso cref="Base.Piece"/> to get the name of.</param>
    /// <param name="name">The resulting name, if found.</param>
    /// <returns>True if the name was found, otherwise false.</returns>
    public static bool TryGetName(Base.Piece pieceType, out string name)
    {
        if (_pieceToString.TryGetValue(pieceType, out name))
        {
            return true;
        }
        return false;
    }
}
