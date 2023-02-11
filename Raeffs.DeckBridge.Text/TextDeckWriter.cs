using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Text;

public abstract class TextDeckWriter : IDeckWriter
{
    private readonly IOptions<CommonOptions> _options;
    private readonly ILogger<TextDeckWriter> _logger;

    public abstract DeckWriterProvider ProviderName { get; }

    public TextDeckWriter(IOptions<CommonOptions> options, ILogger<TextDeckWriter> logger)
    {
        _options = options;
        _logger = logger;
    }

    public async Task WriteDeckAsync(Stream stream, Deck deck, IAsyncEnumerable<Card> cards, CancellationToken cancellationToken = default)
    {
        await using var writer = new StreamWriter(stream);

        await foreach (var card in cards.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            await writer.WriteLineAsync(ConvertCardToLine(card)).ConfigureAwait(false);
        }

        await writer.FlushAsync().ConfigureAwait(false);
    }

    public async Task WriteMultipleDecksAsync(string destination, IAsyncEnumerable<DeckWithCards> decks, CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(destination))
        {
            Directory.CreateDirectory(destination);
        }

        await foreach (var deck in decks.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            var destinationFile = Path.Join(destination, $"{deck.Name}.txt");
            if (!_options.Value.Force && File.Exists(destinationFile))
            {
                throw new ArgumentException($"The file '{destinationFile}' does already exist!");
            }

            _logger.LogInformation("Writing deck {DeckName} to {Destination}", deck.Name, destinationFile);
            await using var stream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write, FileShare.None);
            await WriteDeckAsync(stream, deck, deck.Cards, cancellationToken).ConfigureAwait(false);
        }
    }

    protected abstract string ConvertCardToLine(Card card);
}
