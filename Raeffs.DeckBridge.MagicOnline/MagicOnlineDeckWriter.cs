using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Csv;

namespace Raeffs.DeckBridge.MagicOnline;

internal class MagicOnlineDeckWriter : CsvDeckWriter<MagicOnlineCardMap>
{
    public override DeckWriterProvider ProviderName => DeckWriterProvider.MagicOnline;

    public MagicOnlineDeckWriter(IOptions<CommonOptions> options, ILogger<MagicOnlineDeckWriter> logger)
        : base(options, logger)
    {
    }
}
