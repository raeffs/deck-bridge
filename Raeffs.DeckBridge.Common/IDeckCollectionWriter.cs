namespace Raeffs.DeckBridge.Common;

public interface IDeckCollectionWriter
{
    DeckWriterProvider ProviderName { get; }

    Task WriteDeckAsync(string destination, DeckWithCards deck, CancellationToken cancellationToken = default);
}
