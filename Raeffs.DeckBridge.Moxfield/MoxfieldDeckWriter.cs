using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Text;

namespace Raeffs.DeckBridge.Moxfield;

internal class MoxfieldDeckWriter : TextDeckWriter
{
    public override DeckWriterProvider ProviderName => DeckWriterProvider.MoxfieldDeck;

    public MoxfieldDeckWriter(IOptions<CommonOptions> options, ILogger<TextDeckWriter> logger)
        : base(options, logger)
    {
    }

    protected override string ConvertCardToLine(Card card)
    {
        var parts = new List<string>
        {
            card.Quantity.ToString(),
            card.Name
        };

        if (!string.IsNullOrWhiteSpace(card.SetCode))
        {
            parts.Add($"({card.SetCode})");
        }

        if (!string.IsNullOrWhiteSpace(card.CollectorNumber))
        {
            parts.Add(card.CollectorNumber);
        }

        if (card.IsFoil)
        {
            parts.Add("*F*");
        }

        return string.Join(' ', parts);
    }
}
