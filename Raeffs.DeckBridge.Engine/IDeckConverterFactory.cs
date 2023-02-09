using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Engine;

public interface IDeckConverterFactory
{
    IDeckConverter CreateConverter(DeckReaderProvider from, DeckWriterProvider to);
}
