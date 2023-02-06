using Microsoft.Extensions.DependencyInjection;
using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.MagicOnline;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMagicOnline(this IServiceCollection services)
    {
        services.AddTransient<IDeckWriter, MagicOnlineDeckWriter>();

        return services;
    }
}
