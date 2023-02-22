using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.DelverLens;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDeckReader<T>(this IServiceCollection services)
        where T : class, IDeckReader
    {
        services.TryAddTransient<T>();
        services.AddTransient<IDeckReader, T>();
        return services;
    }

    public static IServiceCollection AddDeckCollectionReader<T>(this IServiceCollection services)
        where T : class, IDeckCollectionReader
    {
        services.TryAddTransient<T>();
        services.AddTransient<IDeckCollectionReader, T>();
        return services;
    }
}
