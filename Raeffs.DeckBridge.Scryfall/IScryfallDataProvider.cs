﻿using Raeffs.DeckBridge.Scryfall.Models;

namespace Raeffs.DeckBridge.Scryfall;

internal interface IScryfallDataProvider
{
    ScryfallCardData? Find(Guid id);
}