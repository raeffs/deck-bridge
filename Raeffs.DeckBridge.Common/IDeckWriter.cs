namespace Raeffs.DeckBridge.Common;

public interface IDeckWriter
{
    DeckWriterProvider ProviderName { get; }

    Task WriteDeckAsync(Stream stream, IAsyncEnumerable<Card> cards, CancellationToken cancellationToken = default);
}
