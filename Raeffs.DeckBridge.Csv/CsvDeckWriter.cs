using CsvHelper;
using CsvHelper.Configuration;
using Raeffs.DeckBridge.Common;
using System.Globalization;

namespace Raeffs.DeckBridge.Csv;

public abstract class CsvDeckWriter<TMap> : IDeckWriter
    where TMap : ClassMap<Card>
{
    private readonly CsvConfiguration _configuration = new(CultureInfo.InvariantCulture);

    public abstract DeckWriterProvider ProviderName { get; }

    public async Task WriteDeckAsync(Stream stream, IAsyncEnumerable<Card> cards, CancellationToken cancellationToken = default)
    {
        await using var writer = new StreamWriter(stream);
        await using var csv = new CsvWriter(writer, _configuration);

        csv.Context.RegisterClassMap<TMap>();

        csv.WriteHeader<Card>();
        await csv.NextRecordAsync().ConfigureAwait(false);

        await foreach (var card in cards.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            csv.WriteRecord(card);
            await csv.NextRecordAsync().ConfigureAwait(false);
        }

        await csv.FlushAsync().ConfigureAwait(false);
    }
}
