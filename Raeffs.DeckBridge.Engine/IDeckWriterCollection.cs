using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Engine;

public interface IDeckWriterCollection
{
    IDeckWriter Find(DeckWriterProvider? providerName);
}
