namespace Raeffs.DeckBridge.Common;

public interface IDeckReader<out T> where T : Card
{
    DeckReaderProvider ProviderName { get; }

    IAsyncEnumerable<T> ReadDeckAsync(string filename, CancellationToken cancellationToken = default);
}
