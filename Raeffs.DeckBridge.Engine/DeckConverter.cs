using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Engine;

internal class DeckConverter : IDeckConverter
{
    private readonly IDeckCollectionReader _collectionReader;
    private readonly IDeckReader _reader;
    private readonly IDeckWriter _writer;

    public DeckConverter(
        IDeckCollectionReader collectionReader,
        IDeckReader reader,
        IDeckWriter writer
    )
    {
        _collectionReader = collectionReader;
        _reader = reader;
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
        true => new CollectionConverterStrategy(_collectionReader, _writer),
        false => throw new NotSupportedException()
    };
}

internal class CollectionConverterStrategy : IDeckConverter
{
    private readonly IDeckCollectionReader _reader;
    private readonly IDeckWriter _writer;

    public CollectionConverterStrategy(IDeckCollectionReader reader, IDeckWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public async Task ConvertDecksAsync(string source, string destination, CancellationToken cancellationToken = default)
    {
        var decks = _reader.ReadDecksAsync(source, cancellationToken);
        await _writer.WriteMultipleDecksAsync(destination, decks, cancellationToken).ConfigureAwait(false);
    }
}
