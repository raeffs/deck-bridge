using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Engine;

internal class NullDeckCollectionReader : IDeckCollectionReader
{
    public DeckReaderProvider ProviderName { get; }

    public bool SupportsMultipleDecksInFile => false;

    public NullDeckCollectionReader(DeckReaderProvider provider)
    {
        ProviderName = provider;
    }

    public IAsyncEnumerable<DeckWithCards> ReadDecksAsync(string source, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
