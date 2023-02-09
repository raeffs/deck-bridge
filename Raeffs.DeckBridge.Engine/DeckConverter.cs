using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Engine;

internal class DeckConverter : IDeckConverter
{
    private readonly IDeckCollectionReader _collectionReader;
    private readonly IDeckReader<Card> _reader;
    private readonly IDeckCollectionWriter _collectionWriter;
    private readonly IDeckWriter _writer;

    public DeckConverter(
        IDeckCollectionReader collectionReader,
        IDeckReader<Card> reader,
        IDeckCollectionWriter collectionWriter,
        IDeckWriter writer
    )
    {
        _collectionReader = collectionReader;
        _reader = reader;
        _collectionWriter = collectionWriter;
        _writer = writer;
    }

    public async Task ConvertDecksAsync(string source, string destination, CancellationToken cancellationToken = default)
    {
        if (!Path.Exists(source))
        {
            throw new Exception();
        }

        var sourceIsFile = File.Exists(source);
        var canReadMultipleDecks = !sourceIsFile || _collectionReader.SupportsMultipleDecksInFile;

        var strategy = SelectStrategy(canReadMultipleDecks);
        await strategy.ConvertDecksAsync(source, destination, cancellationToken).ConfigureAwait(false);
    }

    private IDeckConverter SelectStrategy(bool canReadMultipleDecks) => canReadMultipleDecks switch
    {
        true => new CollectionConverterStrategy(_collectionReader, _collectionWriter),
        false => throw new NotSupportedException()
    };
}

internal class CollectionConverterStrategy : IDeckConverter
{
    private readonly IDeckCollectionReader _reader;
    private readonly IDeckCollectionWriter _writer;

    public CollectionConverterStrategy(IDeckCollectionReader reader, IDeckCollectionWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public async Task ConvertDecksAsync(string source, string destination, CancellationToken cancellationToken = default)
    {
        await foreach (var deck in _reader.ReadDecksAsync(source, cancellationToken).ConfigureAwait(false))
        {
            await _writer.WriteDeckAsync(destination, deck, cancellationToken).ConfigureAwait(false);
        }
    }
}
