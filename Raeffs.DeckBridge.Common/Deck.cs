namespace Raeffs.DeckBridge.Common;

public record Deck
{
    public static readonly Deck AllCards = new();

    public string Id { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;
}
