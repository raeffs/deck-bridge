using Microsoft.Extensions.Options;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Csv;

namespace Raeffs.DeckBridge.Deckstats;

internal class DeckstatsDeckWriter : CsvDeckWriter<DeckstatsCardMap>
{
    public DeckstatsDeckWriter(IOptions<CommonOptions> options) : base(options)
    {
    }

    public override DeckWriterProvider ProviderName => DeckWriterProvider.Deckstats;
}
