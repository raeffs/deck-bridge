using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Scryfall.Models;
using Raeffs.DeckBridge.Scryfall.Options;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Raeffs.DeckBridge.Scryfall;

internal class ScryfallDataProvider : IAppInitializer, IScryfallDataProvider
{
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    private readonly IOptions<ScryfallOptions> _options;
    private readonly ILogger<ScryfallDataProvider> _logger;
    private readonly HttpClient _httpClient;

    private IEnumerable<ScryfallCardData> _data = null!;

    public ScryfallDataProvider(IOptions<ScryfallOptions> options, ILogger<ScryfallDataProvider> logger, HttpClient httpClient)
    {
        _options = options;
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(_options.Value.BulkDataFile))
        {
            _logger.LogInformation("Scryfall bulk data file does not exist at {BulkDataFile}, it will be downloaded. This can take a while.", _options.Value.BulkDataFile);
            await DownloadDataAsync(cancellationToken).ConfigureAwait(false);
        }

        _logger.LogDebug("Loading scryfall data");

        var data = new List<ScryfallCardData>();

        using var stream = File.OpenRead(_options.Value.BulkDataFile);
        await foreach (var entry in JsonSerializer.DeserializeAsyncEnumerable<ScryfallCardData>(stream, _serializerOptions, cancellationToken).ConfigureAwait(false))
        {
            if (entry is not null)
            {
                data.Add(entry);
            }
        }

        _data = data;

        _logger.LogDebug("Scryfall data loaded");
    }

    public ScryfallCardData? Find(Guid id)
    {
        return _data.SingleOrDefault(x => x.Id == id);
    }

    public ScryfallCardData? Find(string setCode, string collectorNumber)
    {
        return _data.SingleOrDefault(x => string.Equals(x.Set, setCode, StringComparison.OrdinalIgnoreCase) && string.Equals(x.CollectorNumber, collectorNumber, StringComparison.OrdinalIgnoreCase));
    }

    private async Task DownloadDataAsync(CancellationToken cancellationToken)
    {
        var info = await _httpClient.GetFromJsonAsync<BulkFileInfo>("https://api.scryfall.com/bulk-data/default-cards", cancellationToken).ConfigureAwait(false);

        using var request = new HttpRequestMessage(HttpMethod.Get, info!.DownloadUri);
        using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        await using var stream = File.OpenWrite(_options.Value.BulkDataFile);
        await response.Content.CopyToAsync(stream, cancellationToken).ConfigureAwait(false);
        await stream.FlushAsync(cancellationToken).ConfigureAwait(false);
    }
}

file record BulkFileInfo
{
    [JsonPropertyName("download_uri")]
    public required string DownloadUri { get; init; }
}
