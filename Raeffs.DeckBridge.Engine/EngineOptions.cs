namespace Raeffs.DeckBridge.Engine;

public record EngineOptions
{
    public required string DelverLensDataFile { get; init; }

    public required string ScryfallBulkDataFile { get; init; }
}
