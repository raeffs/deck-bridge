using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Engine;

internal class DeckWriterCollection : IDeckWriterCollection
{
    private readonly IEnumerable<IDeckWriter> _writers;

    public DeckWriterCollection(IEnumerable<IDeckWriter> writers)
    {
        _writers = writers;
    }

    public IDeckWriter Find(DeckWriterProvider? providerName)
    {
        return _writers.SingleOrDefault(x => x.ProviderName == providerName)
            ?? _writers.Single(x => x.ProviderName == DeckWriterProvider.Generic);
    }
}
