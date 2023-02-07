using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Engine;

public interface IDeckReaderCollection
{
    IDeckReader<Card> Find(DeckReaderProvider? providerName);
}
