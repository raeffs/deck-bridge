using Microsoft.Extensions.Options;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Text;

namespace Raeffs.DeckBridge.Deckstats;

internal class DeckstatsDeckWriter : TextDeckWriter
{
    public override DeckWriterProvider ProviderName => DeckWriterProvider.DeckstatsDeck;

    public DeckstatsDeckWriter(IOptions<CommonOptions> options)
        : base(options)
    {
    }

    protected override string ConvertCardToLine(Card card)
    {
        var parts = new List<string>
        {
            card.Quantity.ToString()
        };

        if (!string.IsNullOrWhiteSpace(card.SetCode))
        {
            parts.Add($"[{card.SetCode}]");
        }

        parts.Add(card.Name);

        var comments = new List<string>();

        if (!string.IsNullOrWhiteSpace(card.Comment))
        {
            comments.Add(card.Comment);
        }

        if (card.IsFoil)
        {
            comments.Add("!Foil");
        }

        if (comments.Any())
        {
            parts.Add("#");
            parts.AddRange(comments);
        }

        return string.Join(' ', parts);
    }
}
