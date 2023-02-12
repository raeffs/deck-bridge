using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Csv;

namespace Raeffs.DeckBridge.Moxfield;

internal class MoxfieldCollectionWriter : CsvDeckWriter<MoxfieldCollectionCardMap>
{
    public override DeckWriterProvider ProviderName => DeckWriterProvider.MoxfieldCollection;

    public MoxfieldCollectionWriter(IOptions<CommonOptions> options, ILogger<MoxfieldCollectionWriter> logger)
        : base(options, logger)
    {
    }
}
