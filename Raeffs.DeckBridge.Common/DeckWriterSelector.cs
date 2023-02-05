namespace Raeffs.DeckBridge.Common;

public class DeckWriterSelector
{
    private readonly IEnumerable<IDeckWriter> _writers;

    public DeckWriterSelector(IEnumerable<IDeckWriter> writers)
    {
        _writers = writers;
    }

    public IDeckWriter SelectWriter(DeckWriterProvider? providerName)
    {
        return _writers.SingleOrDefault(x => x.ProviderName == providerName)
            ?? _writers.Single(x => x.ProviderName == DeckWriterProvider.Generic);
    }
}
