namespace Raeffs.DeckBridge.Common;

public record CommonOptions
{
    public required bool Force { get; init; }
    public required bool Combine { get; init; }
    public required Language DefaultLanguage { get; init; }
}
