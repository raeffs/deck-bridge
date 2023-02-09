namespace Raeffs.DeckBridge.Common;

public interface IDeckWriter
{
    DeckWriterProvider ProviderName { get; }

    Task WriteDeckAsync(Stream stream, IAsyncEnumerable<Card> cards, CancellationToken cancellationToken = default)
        => WriteDeckAsync(stream, Deck.AllCards, cards, cancellationToken);

    Task WriteDeckAsync(Stream stream, Deck deck, IAsyncEnumerable<Card> cards, CancellationToken cancellationToken = default);
}
