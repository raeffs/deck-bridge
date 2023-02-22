using Microsoft.Extensions.Options;
using Raeffs.DeckBridge.Common;
using System.Runtime.CompilerServices;

namespace Raeffs.DeckBridge.Engine;

internal class DefaultValueDecorator : IDeckReader
{
    private readonly IDeckReader _underlyingReader;
    private readonly IOptions<CommonOptions> _options;

    public DeckReaderProvider ProviderName => _underlyingReader.ProviderName;

    public DefaultValueDecorator(IDeckReader underlyingReader, IOptions<CommonOptions> options)
    {
        _underlyingReader = underlyingReader;
        _options = options;
    }

    public async IAsyncEnumerable<Card> ReadDeckAsync(string filename, Deck deck, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var card in _underlyingReader.ReadDeckAsync(filename, deck, cancellationToken).ConfigureAwait(false))
        {
            yield return card with
            {
                Language = card.Language == Language.Unknown
                    ? _options.Value.DefaultLanguage
                    : card.Language
            };
        }
    }
}
