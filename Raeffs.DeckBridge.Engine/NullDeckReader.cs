using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Engine;

internal class NullDeckReader : IDeckReader
{
    public DeckReaderProvider ProviderName { get; }

    public NullDeckReader(DeckReaderProvider provider)
    {
        ProviderName = provider;
    }

    public IAsyncEnumerable<Card> ReadDeckAsync(string filename, Deck deck, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
