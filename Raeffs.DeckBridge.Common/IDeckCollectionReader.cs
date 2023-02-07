namespace Raeffs.DeckBridge.Common;

public interface IDeckCollectionReader
{
    DeckReaderProvider ProviderName { get; }

    IAsyncEnumerable<Deck> ReadDecksAsync(string filename, CancellationToken cancellationToken = default);
}
