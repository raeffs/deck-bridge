using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Raeffs.DeckBridge.Common;
using System.Globalization;

namespace Raeffs.DeckBridge.Csv;

public abstract class CsvDeckWriter<TMap> : IDeckWriter
    where TMap : ClassMap<Card>
{
    private readonly CsvConfiguration _configuration = new(CultureInfo.InvariantCulture);
    private readonly IOptions<CommonOptions> _options;
    private readonly ILogger<CsvDeckWriter<TMap>> _logger;

    public abstract DeckWriterProvider ProviderName { get; }

    public CsvDeckWriter(IOptions<CommonOptions> options, ILogger<CsvDeckWriter<TMap>> logger)
    {
        _options = options;
        _logger = logger;
    }

    public async Task WriteDeckAsync(Stream stream, Deck deck, IAsyncEnumerable<Card> cards, CancellationToken cancellationToken = default)
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

    public async Task WriteMultipleDecksAsync(string destination, IAsyncEnumerable<DeckWithCards> decks, CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(destination))
        {
            Directory.CreateDirectory(destination);
        }

        await foreach (var deck in decks.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            var destinationFile = Path.Join(destination, $"{deck.Name}.csv");
            if (!_options.Value.Force && File.Exists(destinationFile))
            {
                throw new ArgumentException($"The file '{destinationFile}' does already exist!");
            }

            _logger.LogInformation("Writing deck {DeckName} to {Destination}", deck.Name, destinationFile);
            await using var stream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write, FileShare.None);
            await WriteDeckAsync(stream, deck, deck.Cards, cancellationToken).ConfigureAwait(false);
        }
    }
}
