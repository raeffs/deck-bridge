using Microsoft.Extensions.Options;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Text;

namespace Raeffs.DeckBridge.Generic;

internal class GenericDeckWriter : TextDeckWriter
{
    public override DeckWriterProvider ProviderName => DeckWriterProvider.Generic;

    public GenericDeckWriter(IOptions<CommonOptions> options)
        : base(options)
    {
    }

    protected override string ConvertCardToLine(Card card) => $"{card.Quantity} {card.Name}";
}
