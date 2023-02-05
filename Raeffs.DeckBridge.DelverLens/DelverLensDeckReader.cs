using Raeffs.DeckBridge.Common;
using System.Data.SQLite;
using System.Runtime.CompilerServices;

namespace Raeffs.DeckBridge.DelverLens;

internal class DelverLensDeckReader : IDeckReader<DelverLensCard>
{
    private const string CardIdColumnName = "card";
    private const int DefaultCardIdIndex = 1;
    private const string IsFoilColumnName = "foil";
    private const int DefaultIsFoilIndex = 2;
    private const string QuantityColumnName = "quantity";
    private const int DefaultQuantityIndex = 4;
    private const string CreationColumnName = "creation";
    private const int DefaultCreationIndex = 6;

    public async IAsyncEnumerable<DelverLensCard> ReadDeckAsync(string filename, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var connection = new SQLiteConnection($"URI=file:{filename};mode=ReadOnly");
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var command = new SQLiteCommand("SELECT * FROM cards", connection);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

        var schema = await reader.GetColumnSchemaAsync(cancellationToken).ConfigureAwait(false);
        var cardIdIndex = schema.GetColumnIndex(CardIdColumnName, DefaultCardIdIndex);
        var isFoilIndex = schema.GetColumnIndex(IsFoilColumnName, DefaultIsFoilIndex);
        var quantityIndex = schema.GetColumnIndex(QuantityColumnName, DefaultQuantityIndex);
        var creationIndex = schema.GetColumnIndex(CreationColumnName, DefaultCreationIndex);

        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            yield return new()
            {
                InternalId = reader.GetInt32(cardIdIndex),
                IsFoil = reader.GetBoolean(isFoilIndex),
                Quantity = reader.GetInt32(quantityIndex),
                Added = DateTimeOffset.FromUnixTimeMilliseconds(reader.GetInt64(creationIndex)).UtcDateTime
            };
        }
    }
}
