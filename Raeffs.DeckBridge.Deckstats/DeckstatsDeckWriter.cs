using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Csv;

namespace Raeffs.DeckBridge.Deckstats;

internal class DeckstatsDeckWriter : CsvDeckWriter<DeckstatsCardMap>
{
    public override DeckWriterProvider ProviderName => DeckWriterProvider.Deckstats;
}
