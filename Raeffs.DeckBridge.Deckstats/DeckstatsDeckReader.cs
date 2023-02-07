using CsvHelper;
using CsvHelper.Configuration;
using Raeffs.DeckBridge.Common;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Raeffs.DeckBridge.Deckstats;

internal class DeckstatsDeckReader : IDeckReader<Card>
{
    private readonly CsvConfiguration _configuration = new(CultureInfo.InvariantCulture);

    public DeckReaderProvider ProviderName => DeckReaderProvider.Deckstats;

    public IAsyncEnumerable<Card> ReadDeckAsync(string filename, CancellationToken cancellationToken = default)
    {
        return ReadDeckAsync(File.OpenRead(filename), cancellationToken);
    }

    public async IAsyncEnumerable<Card> ReadDeckAsync(Stream stream, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, _configuration);

        csv.Context.RegisterClassMap<DeckstatsCardMap>();

        await foreach (var card in csv.GetRecordsAsync<Card>(cancellationToken).ConfigureAwait(false))
        {
            yield return card;
        }
    }
}
