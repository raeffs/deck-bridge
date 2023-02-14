using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Scryfall;

namespace Raeffs.DeckBridge.DelverLens;

internal record DelverLensCard : Card, IScryfallCardReference
{
    public Guid ScryfallId { get; init; }
}
