using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Scryfall.Models;
using System.Runtime.CompilerServices;

namespace Raeffs.DeckBridge.Scryfall;

internal class ScryfallDataEnricher<T> : IDeckReader<T> where T : Card
{
    private readonly IDeckReader<T> _underlyingReader;
    private readonly IScryfallDataProvider _dataProvider;

    public ScryfallDataEnricher(IDeckReader<T> underlyingReader, IScryfallDataProvider dataProvider)
    {
        _underlyingReader = underlyingReader;
        _dataProvider = dataProvider;
    }

    public DeckReaderProvider ProviderName => _underlyingReader.ProviderName;

    public async IAsyncEnumerable<T> ReadDeckAsync(string filename, Deck deck, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var card in _underlyingReader.ReadDeckAsync(filename, deck, cancellationToken).ConfigureAwait(false))
        {
            yield return EnrichCard(card);
        }
    }

    private T EnrichCard(T card)
    {
        var additionalData = default(ScryfallCardData?);

        if (card is IScryfallCardReference cardWithReference && cardWithReference.ScryfallId != Guid.Empty)
        {
            additionalData = _dataProvider.Find(cardWithReference.ScryfallId);
        }
        else if (!string.IsNullOrWhiteSpace(card.SetCode) && !string.IsNullOrWhiteSpace(card.CollectorNumber))
        {
            additionalData = _dataProvider.Find(card.SetCode, card.CollectorNumber);
        }

        return additionalData is null
            ? card
            : (card with
            {
                Name = additionalData.Name,
                SetCode = additionalData.Set,
                CollectorNumber = additionalData.CollectorNumber,
                Rarity = additionalData.Rarity
            });
    }
}
