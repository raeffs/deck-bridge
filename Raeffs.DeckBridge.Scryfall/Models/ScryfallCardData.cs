using System.Text.Json.Serialization;

namespace Raeffs.DeckBridge.Scryfall.Models;

internal record ScryfallCardData
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Set { get; init; } = string.Empty;

    [JsonPropertyName("collector_number")]
    public string CollectorNumber { get; init; } = string.Empty;
}
