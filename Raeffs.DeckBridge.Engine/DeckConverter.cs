using Microsoft.Extensions.Options;
using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Engine;

internal class DeckConverter : IDeckConverter
{
    private readonly IOptions<CommonOptions> _options;
    private readonly IDeckCollectionReader _collectionReader;
    private readonly IDeckReader _reader;
    private readonly IDeckWriter _writer;

    public DeckConverter(
        IOptions<CommonOptions> options,
        IDeckCollectionReader collectionReader,
        IDeckReader reader,
        IDeckWriter writer
    )
    {
        _options = options;
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
        var combineOutput = _options.Value.Combine;

        var strategy = SelectStrategy(canReadMultipleDecks, combineOutput);
        await strategy.ConvertDecksAsync(source, destination, cancellationToken).ConfigureAwait(false);
    }

    private IDeckConverter SelectStrategy(bool canReadMultipleDecks, bool combineOutput) => (canReadMultipleDecks, combineOutput) switch
    {
        (true, false) => new CollectionConverterStrategy(_collectionReader, _writer),
        (true, true) => new CollectionCombineStrategy(_reader, _writer),
        _ => new DeckConverterStrategy(_reader, _writer)
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

internal class DeckConverterStrategy : IDeckConverter
{
    private readonly IDeckReader _reader;
    private readonly IDeckWriter _writer;

    public DeckConverterStrategy(IDeckReader reader, IDeckWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public async Task ConvertDecksAsync(string source, string destination, CancellationToken cancellationToken = default)
    {
        var cards = _reader.ReadDeckAsync(source, Deck.AllCards, cancellationToken);
        await using var stream = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None);
        await _writer.WriteDeckAsync(stream, Deck.AllCards, cards, cancellationToken).ConfigureAwait(false);
    }
}

internal class CollectionCombineStrategy : IDeckConverter
{
    private readonly IDeckReader _reader;
    private readonly IDeckWriter _writer;

    public CollectionCombineStrategy(IDeckReader reader, IDeckWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public async Task ConvertDecksAsync(string source, string destination, CancellationToken cancellationToken = default)
    {
        // todo: should use collection reader
        var deck = _reader.ReadDeckAsync(source, Deck.AllCards, cancellationToken);
        await using var stream = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None);
        await _writer.WriteDeckAsync(stream, deck, cancellationToken).ConfigureAwait(false);
    }
}
