using Microsoft.Extensions.Options;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Csv;

namespace Raeffs.DeckBridge.Moxfield;

internal class MoxfieldDeckWriter : CsvDeckWriter<MoxfieldCardMap>
{
    public MoxfieldDeckWriter(IOptions<CommonOptions> options) : base(options)
    {
    }

    public override DeckWriterProvider ProviderName => DeckWriterProvider.Moxfield;
}
