using CsvHelper;
using CsvHelper.Configuration;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Deckstats.Models;
using System.Globalization;

namespace Raeffs.DeckBridge.Deckstats;

internal class DeckstatsDeckWriter : IDeckWriter
{
    private readonly CsvConfiguration _configuration = new(CultureInfo.InvariantCulture)
    {

    };

    public DeckWriterProvider ProviderName => DeckWriterProvider.Deckstats;

    public async Task WriteDeckAsync(Stream stream, IAsyncEnumerable<Card> cards, CancellationToken cancellationToken = default)
    {
        await using var writer = new StreamWriter(stream);
        await using var csv = new CsvWriter(writer, _configuration);

        csv.WriteHeader<DeckstatsCardData>();
        await csv.NextRecordAsync().ConfigureAwait(false);

        await foreach (var card in cards.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            csv.WriteRecord(new DeckstatsCardData
            {
                Name = card.Name,
                Quantity = card.Quantity,
                IsFoil = card.IsFoil,
                SetCode = card.SetCode,
                CollectorNumber = card.CollectorNumber
            });
            await csv.NextRecordAsync().ConfigureAwait(false);
        }

        await csv.FlushAsync().ConfigureAwait(false);
    }
}
