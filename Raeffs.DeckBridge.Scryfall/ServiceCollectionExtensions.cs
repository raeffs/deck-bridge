using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Scryfall.Options;

namespace Raeffs.DeckBridge.Scryfall;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddScryfall(this IServiceCollection services, IConfigurationSection configuration)
    {
        services.Configure<ScryfallOptions>(configuration);

        services.AddSingleton<ScryfallDataProvider>();
        services.AddTransient<IAppInitializer>(services => services.GetRequiredService<ScryfallDataProvider>());
        services.AddTransient<IScryfallDataProvider>(services => services.GetRequiredService<ScryfallDataProvider>());

        services.Decorate<IDeckReader, ScryfallDataEnricher>();

        return services;
    }
}
