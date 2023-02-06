using System.Data.Common;

namespace Raeffs.DeckBridge.DelverLens;

internal static class DbColumnExtensions
{
    public static int GetColumnIndex(this IEnumerable<DbColumn> schema, string columnName, int defaultIndex)
    {
        return schema.Single(x => x.ColumnName == columnName).ColumnOrdinal.GetValueOrDefault(defaultIndex);
    }

    public static int GetColumnIndex(this IEnumerable<DbColumn> schema, ColumnDefinition definition)
    {
        return schema.GetColumnIndex(definition.ColumnName, definition.DefaultIndex);
    }
}

public record ColumnDefinition(string ColumnName, int DefaultIndex);
