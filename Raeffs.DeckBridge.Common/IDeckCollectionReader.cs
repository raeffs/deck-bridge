namespace Raeffs.DeckBridge.Common;

public interface IDeckCollectionReader
{
    DeckReaderProvider ProviderName { get; }

    bool SupportsMultipleDecksInFile { get; }

    IAsyncEnumerable<DeckWithCards> ReadDecksAsync(string source, CancellationToken cancellationToken = default);
}
