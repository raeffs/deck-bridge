using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Deckstats;

internal class DeckstatsDeckCollectionWriter : IDeckCollectionWriter
{
    private readonly IDeckWriter _writer;

    public DeckWriterProvider ProviderName => DeckWriterProvider.Deckstats;

    public DeckstatsDeckCollectionWriter(IEnumerable<IDeckWriter> writers)
    {
        _writer = writers.Single(x => x.ProviderName == ProviderName);
    }

    public Task WriteDeckAsync(string destination, DeckWithCards deck, CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(destination))
        {
            Directory.CreateDirectory(destination);
        }

        var destinationFile = Path.Join(destination, $"{deck.Name}.csv");
        if (File.Exists(destinationFile))
        {
            throw new ArgumentException($"The file '{destinationFile}' does already exist!");
        }

        return InternalWriteDeckAsync(destinationFile, deck, cancellationToken);
    }

    private async Task InternalWriteDeckAsync(string destinationFile, DeckWithCards deck, CancellationToken cancellationToken = default)
    {
        await using var stream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write, FileShare.None);
        await _writer.WriteDeckAsync(stream, deck, deck.Cards, cancellationToken).ConfigureAwait(false);
    }
}
