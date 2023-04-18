namespace Raeffs.DeckBridge.Common;

public record Card
{
    public required string OriginalId { get; init; }

    public int Quantity { get; init; }

    public Language Language { get; init; }

    public Condition Condition { get; init; }

    public bool IsFoil { get; init; }

    public bool IsSigned { get; init; }

    public bool IsPinned { get; init; }

    public DateTime? Added { get; init; }

    public string Comment { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string SetCode { get; init; } = string.Empty;

    public string CollectorNumber { get; init; } = string.Empty;

    public Rarity Rarity { get; init; }

    public Guid ScryfallId { get; init; }
}
