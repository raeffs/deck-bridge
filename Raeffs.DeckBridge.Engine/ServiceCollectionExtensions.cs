using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raeffs.DeckBridge.Deckstats;
using Raeffs.DeckBridge.DelverLens;
using Raeffs.DeckBridge.Generic;
using Raeffs.DeckBridge.MagicOnline;
using Raeffs.DeckBridge.Moxfield;
using Raeffs.DeckBridge.Scryfall;

namespace Raeffs.DeckBridge.Engine;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDeckBridgeEngine(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddGeneric()
            .AddDelverLens(configuration.GetSection("DelverLens"))
            .AddScryfall(configuration.GetSection("Scryfall"))
            .AddDeckstats()
            .AddMoxfield()
            .AddMagicOnline();

        services
            .AddTransient<IDeckReaderCollection, DeckReaderCollection>()
            .AddTransient<IDeckWriterCollection, DeckWriterCollection>()
            .AddTransient<IDeckConverterFactory, DeckConverterFactory>();

        return services;
    }

    public static IServiceCollection AddDeckBridgeEngine(this IServiceCollection services, EngineOptions options)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "Scryfall:BulkDataFile", options.ScryfallBulkDataFile },
                { "DelverLens:DataFile", options.DelverLensDataFile }
            })
            .Build();

        return services.AddDeckBridgeEngine(configuration);
    }
}
