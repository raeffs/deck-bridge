using Microsoft.Extensions.DependencyInjection;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Scryfall;

namespace Raeffs.DeckBridge.Deckstats;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDeckstats(this IServiceCollection services)
    {
        services.AddTransient<IDeckWriter, DeckstatsDeckWriter>();

        services.AddTransient<DeckstatsDeckReader>();
        services.AddDeckReaderWithScryfallData<DeckstatsDeckReader, Card>();

        return services;
    }
}
