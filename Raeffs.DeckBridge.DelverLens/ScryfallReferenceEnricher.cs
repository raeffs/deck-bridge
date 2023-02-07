using Microsoft.Extensions.Options;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.DelverLens.Options;
using System.Data.SQLite;
using System.Runtime.CompilerServices;

namespace Raeffs.DeckBridge.DelverLens;

internal class ScryfallReferenceEnricher : IDeckReader<DelverLensCard>
{
    private readonly IDeckReader<DelverLensCard> _underlyingReader;
    private readonly IOptions<DelverLensOptions> _options;

    public ScryfallReferenceEnricher(IDeckReader<DelverLensCard> underlyingReader, IOptions<DelverLensOptions> options)
    {
        _underlyingReader = underlyingReader;
        _options = options;
    }

    public DeckReaderProvider ProviderName => _underlyingReader.ProviderName;

    public async IAsyncEnumerable<DelverLensCard> ReadDeckAsync(string filename, Deck deck, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var connection = new SQLiteConnection($"URI=file:{_options.Value.DataFile};mode=ReadOnly");
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        await foreach (var card in _underlyingReader.ReadDeckAsync(filename, deck, cancellationToken).ConfigureAwait(false))
        {
            using var command = new SQLiteCommand("SELECT scryfall_id FROM cards WHERE _id=@Id", connection);
            command.Parameters.AddWithValue("@Id", card.InternalId);
            await command.PrepareAsync(cancellationToken).ConfigureAwait(false);

            var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
            var scryfallId = Guid.Parse(Convert.ToString(result)!);

            yield return card with
            {
                ScryfallId = scryfallId
            };
        }
    }
}
