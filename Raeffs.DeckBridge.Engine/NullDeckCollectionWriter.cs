using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Engine;

internal class NullDeckCollectionWriter : IDeckCollectionWriter
{
    public DeckWriterProvider ProviderName { get; }

    public NullDeckCollectionWriter(DeckWriterProvider provider)
    {
        ProviderName = provider;
    }

    public Task WriteDeckAsync(string destination, DeckWithCards deck, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
