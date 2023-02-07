using Microsoft.Extensions.Options;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Scryfall.Models;
using Raeffs.DeckBridge.Scryfall.Options;
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

    private IEnumerable<ScryfallCardData> _data = null!;

    public ScryfallDataProvider(IOptions<ScryfallOptions> options)
    {
        _options = options;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
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
    }

    public ScryfallCardData? Find(Guid id)
    {
        return _data.SingleOrDefault(x => x.Id == id);
    }

    public ScryfallCardData? Find(string setCode, string collectorNumber)
    {
        return _data.SingleOrDefault(x => string.Equals(x.Set, setCode, StringComparison.OrdinalIgnoreCase) && string.Equals(x.CollectorNumber, collectorNumber, StringComparison.OrdinalIgnoreCase));
    }
}
