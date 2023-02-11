using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Engine;

internal class NullDeckWriter : IDeckWriter
{
    public DeckWriterProvider ProviderName { get; }

    public NullDeckWriter(DeckWriterProvider provider)
    {
        ProviderName = provider;
    }

    public Task WriteDeckAsync(Stream stream, Deck deck, IAsyncEnumerable<Card> cards, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task WriteMultipleDecksAsync(string destination, IAsyncEnumerable<DeckWithCards> decks, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
