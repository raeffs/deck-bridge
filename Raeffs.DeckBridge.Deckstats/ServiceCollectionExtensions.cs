using Microsoft.Extensions.DependencyInjection;
using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Deckstats;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDeckstats(this IServiceCollection services)
    {
        services.AddTransient<IDeckWriter, DeckstatsCollectionWriter>();
        services.AddTransient<IDeckWriter, DeckstatsDeckWriter>();

        return services;
    }
}
