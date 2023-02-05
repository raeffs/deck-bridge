namespace Raeffs.DeckBridge.Common;

public record Card
{
    public bool IsFoil { get; init; }

    public int Quantity { get; init; }

    public Language Language { get; init; }

    public Condition Condition { get; init; }

    public DateTime Added { get; init; }

    public string Comment { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string SetCode { get; init; } = string.Empty;

    public string CollectorNumber { get; init; } = string.Empty;
}
