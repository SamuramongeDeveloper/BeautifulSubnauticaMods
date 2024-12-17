using Nautilus.Handlers;
using System.Collections.Generic;

namespace Baselib.Handlers;

/// <summary>
/// Handles the creation of new <seealso cref="Base.CellType"/>s.
/// </summary>
public static class CustomCellTypesHandler
{
    private static readonly Dictionary<Base.CellType, string> _cellTypeToString = new();

    /// <summary>
    /// A list holding all the custom <seealso cref="Base.CellType"/>s.
    /// </summary>
    public static List<Base.CellType> Cells { get; } = new();

    /// <summary>
    /// Tries to add a custom <seealso cref="Base.CellType"/>.
    /// </summary>
    /// <param name="name">The name of the custom <seealso cref="Base.CellType"/>.</param>
    /// <param name="result">The resulting <seealso cref="Base.CellType"/> if added correctly.</param>
    /// <returns>True if added correctly, otherwise false.</returns>
    public static bool TryAdd(string name, out Base.CellType result)
    {
        if (EnumHandler.TryAddEntry(name, out EnumBuilder<Base.CellType> cellTypeBuilder))
        {
            _cellTypeToString.Add(cellTypeBuilder, name);
            Cells.Add(cellTypeBuilder);
            result = cellTypeBuilder;
            return true;
        }
        result = Base.CellType.Empty;
        return false;
    }

    /// <summary>
    /// Tries to get a custom <seealso cref="Base.CellType"/> from a name.
    /// </summary>
    /// <param name="name">The name of the <seealso cref="Base.CellType"/>.</param>
    /// <param name="result">The resulting <seealso cref="Base.CellType"/>.</param>
    /// <returns>True if the <seealso cref="Base.CellType"/> exists, otherwise false.</returns>
    public static bool TryGet(string name, out Base.CellType result)
    {
        if (EnumHandler.TryGetValue(name, out result))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Tries to get the name of a custom <seealso cref="Base.CellType"/>.
    /// </summary>
    /// <param name="cellType">The <seealso cref="Base.CellType"/> to get the name of.</param>
    /// <param name="name">The resulting name, if found.</param>
    /// <returns>True if the name was found, otherwise false.</returns>
    public static bool TryGetName(Base.CellType cellType, out string name)
    {
        if (_cellTypeToString.TryGetValue(cellType, out name))
        {
            return true;
        }
        return false;
    }
}
