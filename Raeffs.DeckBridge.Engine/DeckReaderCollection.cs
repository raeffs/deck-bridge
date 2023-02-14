using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Engine;

internal class DeckReaderCollection : IDeckReaderCollection
{
    private readonly IEnumerable<IDeckReader> _readers;

    public DeckReaderCollection(IEnumerable<IDeckReader> readers)
    {
        _readers = readers;
    }

    public IDeckReader Find(DeckReaderProvider? providerName)
    {
        return _readers.SingleOrDefault(x => x.ProviderName == providerName)!;
    }
}
