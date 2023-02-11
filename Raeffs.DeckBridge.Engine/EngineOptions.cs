using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Engine;

public record EngineOptions : CommonOptions
{
    public required string DelverLensDataFile { get; init; }

    public required string ScryfallBulkDataFile { get; init; }
}
