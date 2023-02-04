namespace Raeffs.DeckBridge.Common;

public interface IDeckWriter
{
    Task WriteDeckAsync(Stream stream, IAsyncEnumerable<Card> cards, CancellationToken cancellationToken = default);
}
