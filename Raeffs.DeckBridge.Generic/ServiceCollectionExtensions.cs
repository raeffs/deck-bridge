using Microsoft.Extensions.DependencyInjection;
using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Generic;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGeneric(this IServiceCollection services)
    {
        services.AddTransient<IDeckWriter, GenericDeckWriter>();

        return services;
    }
}
