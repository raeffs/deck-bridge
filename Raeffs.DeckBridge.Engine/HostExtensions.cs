using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Engine;

public static class HostExtensions
{
    public static async Task InitializeEngineAsync(this IHost host, CancellationToken cancellationToken = default)
    {
        await using var scope = host.Services.CreateAsyncScope();

        var initializers = scope.ServiceProvider.GetRequiredService<IEnumerable<IAppInitializer>>();
        foreach (var initializer in initializers)
        {
            await initializer.InitializeAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
