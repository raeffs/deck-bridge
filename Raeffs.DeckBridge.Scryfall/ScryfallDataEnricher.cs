using Raeffs.DeckBridge.Common;
using System.Runtime.CompilerServices;

namespace Raeffs.DeckBridge.Scryfall;

internal class ScryfallDataEnricher<T> : IDeckReader<T> where T : Card, IScryfallCardReference
{
    private readonly IDeckReader<T> _underlyingReader;
    private readonly IScryfallDataProvider _dataProvider;

    public ScryfallDataEnricher(IDeckReader<T> underlyingReader, IScryfallDataProvider dataProvider)
    {
        _underlyingReader = underlyingReader;
        _dataProvider = dataProvider;
    }

    public async IAsyncEnumerable<T> ReadDeckAsync(string filename, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var card in _underlyingReader.ReadDeckAsync(filename, cancellationToken).ConfigureAwait(false))
        {
            if (card.ScryfallId == Guid.Empty)
            {
                yield return card;
                continue;
            }

            var data = _dataProvider.Find(card.ScryfallId);
            if (data is null)
            {
                yield return card;
                continue;
            }

            yield return card with
            {
                Name = data.Name,
                SetCode = data.Set,
                CollectorNumber = data.CollectorNumber,
                Rarity = data.Rarity
            };
        }
    }
}
