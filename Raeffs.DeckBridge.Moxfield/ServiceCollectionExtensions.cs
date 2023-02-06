using Microsoft.Extensions.DependencyInjection;
using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Moxfield;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMoxfield(this IServiceCollection services)
    {
        services.AddTransient<IDeckWriter, MoxfieldDeckWriter>();

        return services;
    }
}
