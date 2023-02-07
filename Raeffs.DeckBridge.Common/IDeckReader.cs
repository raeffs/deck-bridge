namespace Raeffs.DeckBridge.Common;

public interface IDeckReader<out T> where T : Card
{
    DeckReaderProvider ProviderName { get; }

    IAsyncEnumerable<T> ReadDeckAsync(string filename, CancellationToken cancellationToken = default)
        => ReadDeckAsync(filename, Deck.AllCards, cancellationToken);

    IAsyncEnumerable<T> ReadDeckAsync(string filename, Deck deck, CancellationToken cancellationToken = default);
}
