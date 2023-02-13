using Raeffs.DeckBridge.Scryfall.Models;

namespace Raeffs.DeckBridge.Scryfall;

internal interface IScryfallDataProvider
{
    Task<ScryfallCardData?> FindAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ScryfallCardData?> FindAsync(string setCode, string collectorNumber, CancellationToken cancellationToken = default);
}
