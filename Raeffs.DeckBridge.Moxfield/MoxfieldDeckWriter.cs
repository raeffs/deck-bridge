using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Csv;

namespace Raeffs.DeckBridge.Moxfield;

internal class MoxfieldDeckWriter : CsvDeckWriter<MoxfieldCardMap>
{
    public override DeckWriterProvider ProviderName => DeckWriterProvider.Moxfield;
}
