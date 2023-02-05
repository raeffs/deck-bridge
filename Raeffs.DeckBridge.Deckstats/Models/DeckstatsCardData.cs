using CsvHelper.Configuration.Attributes;

namespace Raeffs.DeckBridge.Deckstats.Models;

internal record DeckstatsCardData
{
    [Name("amount")]
    public int Quantity { get; init; }

    [Name("card_name")]
    public string Name { get; init; } = string.Empty;

    [Name("is_foil")]
    public bool IsFoil { get; init; }

    [Name("set_name")]
    public string SetCode { get; init; } = string.Empty;

    [Name("collector_number")]
    public string CollectorNumber { get; init; } = string.Empty;
}
