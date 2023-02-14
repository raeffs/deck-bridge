using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Raeffs.DeckBridge.Common;
using System.Text.Json;

namespace Raeffs.DeckBridge.DelverLens;

internal class DelverLensDataProvider : IAppInitializer, IDelverLensDataProvider
{
    private readonly IOptions<DelverLensOptions> _options;
    private readonly ILogger<DelverLensDataProvider> _logger;

    private IEnumerable<Mapping> _data = null!;

    public DelverLensDataProvider(IOptions<DelverLensOptions> options, ILogger<DelverLensDataProvider> logger)
    {
        _options = options;
        _logger = logger;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(_options.Value.DataFile))
        {
            throw new Exception($"The DelverLens data file '{_options.Value.DataFile}' does not exist or cannot be accessed!");
        }

        _logger.LogDebug("Loading DelverLens data");

        var data = new List<Mapping>();

        using var stream = File.OpenRead(_options.Value.DataFile);
        await foreach (var entry in JsonSerializer.DeserializeAsyncEnumerable<Mapping>(stream, cancellationToken: cancellationToken).ConfigureAwait(false))
        {
            if (entry is not null)
            {
                data.Add(entry);
            }
        }

        _data = data;

        _logger.LogDebug("DelverLens data loaded");
    }

    public Guid Find(int id) => _data.SingleOrDefault(x => x.Id == id)?.ScryfallId ?? Guid.Empty;
}

internal class Mapping
{
    public int Id { get; init; }
    public Guid ScryfallId { get; init; }
}
