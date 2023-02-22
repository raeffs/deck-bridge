using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.DelverLens;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDelverLens(this IServiceCollection services, IConfigurationSection configuration)
    {
        services.Configure<DelverLensOptions>(configuration);

        services.AddSingleton<DelverLensDataProvider>();
        services.AddTransient<IAppInitializer>(services => services.GetRequiredService<DelverLensDataProvider>());
        services.AddTransient<IDelverLensDataProvider>(services => services.GetRequiredService<DelverLensDataProvider>());

        services.AddDeckReader<DelverLensDeckReader>();
        services.AddDeckCollectionReader<DelverLensDeckCollectionReader>();

        return services;
    }
}
