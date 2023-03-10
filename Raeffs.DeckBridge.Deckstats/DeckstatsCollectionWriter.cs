using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Csv;

namespace Raeffs.DeckBridge.Deckstats;

internal class DeckstatsCollectionWriter : CsvDeckWriter<DeckstatsCollectionCardMap>
{
    public override DeckWriterProvider ProviderName => DeckWriterProvider.DeckstatsCollection;

    public DeckstatsCollectionWriter(IOptions<CommonOptions> options, ILogger<DeckstatsCollectionWriter> logger)
        : base(options, logger)
    {
    }
}
