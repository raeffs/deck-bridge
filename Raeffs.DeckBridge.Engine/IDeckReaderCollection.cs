using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Engine;

public interface IDeckReaderCollection
{
    IDeckReader Find(DeckReaderProvider? providerName);
}
