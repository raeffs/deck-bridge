namespace Raeffs.DeckBridge.Common;

public interface IDeckReader
{
    DeckReaderProvider ProviderName { get; }

    IAsyncEnumerable<Card> ReadDeckAsync(string filename, CancellationToken cancellationToken = default)
        => ReadDeckAsync(filename, Deck.AllCards, cancellationToken);

    IAsyncEnumerable<Card> ReadDeckAsync(string filename, Deck deck, CancellationToken cancellationToken = default);
}
