using Raeffs.DeckBridge.Common;
using System.Data.SQLite;
using System.Runtime.CompilerServices;

namespace Raeffs.DeckBridge.DelverLens;

internal class DelverLensDeckCollectionReader : IDeckCollectionReader
{
    private static readonly ColumnDefinition IdColumn = new("_id", 0);
    private static readonly ColumnDefinition NameColumn = new("name", 3);

    public DeckReaderProvider ProviderName => DeckReaderProvider.DelverLens;

    public async IAsyncEnumerable<Deck> ReadDecksAsync(string filename, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var connection = new SQLiteConnection($"URI=file:{filename};mode=ReadOnly");
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var command = new SQLiteCommand("SELECT * FROM lists", connection);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

        var schema = await reader.GetColumnSchemaAsync(cancellationToken).ConfigureAwait(false);
        var idIndex = schema.GetColumnIndex(IdColumn);
        var nameIndex = schema.GetColumnIndex(NameColumn);

        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            yield return new()
            {
                Id = reader.GetInt32(idIndex).ToString(),
                Name = reader.GetString(nameIndex),
            };
        }
    }
}
