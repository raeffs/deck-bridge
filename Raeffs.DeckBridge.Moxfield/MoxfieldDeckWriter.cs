using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Csv;

namespace Raeffs.DeckBridge.Moxfield;

internal class MoxfieldDeckWriter : CsvDeckWriter<MoxfieldCardMap>
{
    public override DeckWriterProvider ProviderName => DeckWriterProvider.Moxfield;

    public MoxfieldDeckWriter(IOptions<CommonOptions> options, ILogger<MoxfieldDeckWriter> logger)
        : base(options, logger)
    {
    }
}
