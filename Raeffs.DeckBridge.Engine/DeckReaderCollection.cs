using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Engine;

internal class DeckReaderCollection : IDeckReaderCollection
{
    private readonly IEnumerable<IDeckReader<Card>> _readers;

    public DeckReaderCollection(IEnumerable<IDeckReader<Card>> readers)
    {
        _readers = readers;
    }

    public IDeckReader<Card> Find(DeckReaderProvider? providerName)
    {
        return _readers.SingleOrDefault(x => x.ProviderName == providerName)!;
    }
}
