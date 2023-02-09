using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Generic;

internal class GenericDeckWriter : IDeckWriter
{
    public DeckWriterProvider ProviderName => DeckWriterProvider.Generic;

    public async Task WriteDeckAsync(Stream stream, Deck deck, IAsyncEnumerable<Card> cards, CancellationToken cancellationToken = default)
    {
        await using var writer = new StreamWriter(stream);

        await foreach (var card in cards.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            await writer.WriteLineAsync($"{card.Quantity}x {card.Name}").ConfigureAwait(false);
        }

        await writer.FlushAsync().ConfigureAwait(false);
    }
}
