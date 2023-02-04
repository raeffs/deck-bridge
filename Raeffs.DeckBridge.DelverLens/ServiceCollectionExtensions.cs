using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Raeffs.DeckBridge.DelverLens.Options;
using Raeffs.DeckBridge.Scryfall;

namespace Raeffs.DeckBridge.DelverLens;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDelverLens(this IServiceCollection services, IConfigurationSection configuration)
    {
        services.Configure<DelverLensOptions>(configuration);

        services.AddTransient<DelverLensDeckReader>();
        services.AddTransient(services => new ScryfallReferenceEnricher(
            services.GetRequiredService<DelverLensDeckReader>(),
            services.GetRequiredService<IOptions<DelverLensOptions>>()
        ));
        services.AddDeckReaderWithScryfallData<ScryfallReferenceEnricher, DelverLensCard>();

        return services;
    }
}
