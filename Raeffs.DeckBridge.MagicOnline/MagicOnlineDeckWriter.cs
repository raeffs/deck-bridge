using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Csv;

namespace Raeffs.DeckBridge.MagicOnline;

internal class MagicOnlineDeckWriter : CsvDeckWriter<MagicOnlineCardMap>
{
    public override DeckWriterProvider ProviderName => DeckWriterProvider.MagicOnline;
}
