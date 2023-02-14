using Raeffs.DeckBridge.Common;
using System.Data.SQLite;
using System.Runtime.CompilerServices;

namespace Raeffs.DeckBridge.DelverLens;

internal class DelverLensDeckCollectionReader : IDeckCollectionReader
{
    private static readonly ColumnDefinition IdColumn = new("_id", 0);
    private static readonly ColumnDefinition NameColumn = new("name", 3);

    private readonly IDeckReader _reader;

    public DeckReaderProvider ProviderName => DeckReaderProvider.DelverLens;

    public bool SupportsMultipleDecksInFile => true;

    public DelverLensDeckCollectionReader(IEnumerable<IDeckReader> readers)
    {
        _reader = readers.Single(x => x.ProviderName == ProviderName);
    }

    public async IAsyncEnumerable<DeckWithCards> ReadDecksAsync(string source, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var connection = new SQLiteConnection($"URI=file:{source};mode=ReadOnly");
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var command = new SQLiteCommand("SELECT * FROM lists", connection);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

        var schema = await reader.GetColumnSchemaAsync(cancellationToken).ConfigureAwait(false);
        var idIndex = schema.GetColumnIndex(IdColumn);
        var nameIndex = schema.GetColumnIndex(NameColumn);

        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            var deck = new Deck
            {
                Id = reader.GetInt32(idIndex).ToString(),
                Name = reader.GetString(nameIndex),
            };

            yield return new()
            {
                Id = deck.Id,
                Name = deck.Name,
                Cards = _reader.ReadDeckAsync(source, deck, cancellationToken)
            };
        }
    }
}
