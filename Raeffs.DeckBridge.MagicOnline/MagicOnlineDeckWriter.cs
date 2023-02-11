using Microsoft.Extensions.Options;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Csv;

namespace Raeffs.DeckBridge.MagicOnline;

internal class MagicOnlineDeckWriter : CsvDeckWriter<MagicOnlineCardMap>
{
    public MagicOnlineDeckWriter(IOptions<CommonOptions> options) : base(options)
    {
    }

    public override DeckWriterProvider ProviderName => DeckWriterProvider.MagicOnline;
}
