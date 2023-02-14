using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Scryfall;

namespace Raeffs.DeckBridge.DelverLens;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDelverLens(this IServiceCollection services, IConfigurationSection configuration)
    {
        services.Configure<DelverLensOptions>(configuration);

        services.AddSingleton<DelverLensDataProvider>();
        services.AddTransient<IAppInitializer>(services => services.GetRequiredService<DelverLensDataProvider>());
        services.AddTransient<IDelverLensDataProvider>(services => services.GetRequiredService<DelverLensDataProvider>());

        services.AddTransient<DelverLensDeckReader>();
        services.AddDeckReaderWithScryfallData<DelverLensDeckReader, DelverLensCard>();

        services.AddTransient<IDeckCollectionReader, DelverLensDeckCollectionReader>();

        return services;
    }
}
