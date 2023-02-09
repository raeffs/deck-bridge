namespace Raeffs.DeckBridge.Common;

public record DeckWithCards : Deck
{
    public required IAsyncEnumerable<Card> Cards { get; init; }
}
