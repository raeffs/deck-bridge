using Raeffs.DeckBridge.Common;
using System.Data.SQLite;
using System.Runtime.CompilerServices;

namespace Raeffs.DeckBridge.DelverLens;

internal class DelverLensDeckReader : IDeckReader
{
    private static readonly ColumnDefinition CardColumn = new("card", 1);
    private static readonly ColumnDefinition FoilColumn = new("foil", 2);
    private static readonly ColumnDefinition QuantityColumn = new("quantity", 4);
    private static readonly ColumnDefinition CreationColumn = new("creation", 6);
    private static readonly ColumnDefinition NoteColumn = new("note", 8);
    private static readonly ColumnDefinition ConditionColumn = new("condition", 9);
    private static readonly ColumnDefinition LanguageColumn = new("language", 10);

    private readonly IDelverLensDataProvider _delverLensDataProvider;

    public DeckReaderProvider ProviderName => DeckReaderProvider.DelverLens;

    public DelverLensDeckReader(IDelverLensDataProvider delverLensDataProvider)
    {
        _delverLensDataProvider = delverLensDataProvider;
    }

    public async IAsyncEnumerable<Card> ReadDeckAsync(string filename, Deck deck, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var connection = new SQLiteConnection($"URI=file:{filename};mode=ReadOnly");
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var command = await GetCommandAsync(connection, deck, cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

        var schema = await reader.GetColumnSchemaAsync(cancellationToken).ConfigureAwait(false);
        var cardIndex = schema.GetColumnIndex(CardColumn);
        var foilIndex = schema.GetColumnIndex(FoilColumn);
        var quantityIndex = schema.GetColumnIndex(QuantityColumn);
        var creationIndex = schema.GetColumnIndex(CreationColumn);
        var noteIndex = schema.GetColumnIndex(NoteColumn);
        var conditionIndex = schema.GetColumnIndex(ConditionColumn);
        var languageIndex = schema.GetColumnIndex(LanguageColumn);

        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            var id = reader.GetInt32(cardIndex);

            yield return new()
            {
                IsFoil = reader.GetBoolean(foilIndex),
                Quantity = reader.GetInt32(quantityIndex),
                Added = DateTimeOffset.FromUnixTimeMilliseconds(reader.GetInt64(creationIndex)).UtcDateTime,
                Comment = reader.GetString(noteIndex),
                Condition = GetCondition(reader.GetString(conditionIndex)),
                Language = GetLanguage(reader.GetString(languageIndex)),
                ScryfallId = _delverLensDataProvider.Find(id)
            };
        }
    }

    private static async Task<SQLiteCommand> GetCommandAsync(SQLiteConnection connection, Deck deck, CancellationToken cancellationToken)
    {
        if (deck == Deck.AllCards)
        {
            return new SQLiteCommand("SELECT * FROM cards", connection);
        }
        else
        {
            var command = new SQLiteCommand("SELECT * FROM cards WHERE list = @DeckId", connection);
            command.Parameters.AddWithValue("@DeckId", deck.Id);
            await command.PrepareAsync(cancellationToken).ConfigureAwait(false);
            return command;
        }
    }

    private static Condition GetCondition(string value) => value switch
    {
        "Near Mint" => Condition.NearMint,
        "Slightly Played" => Condition.Excellent,
        "Moderately Played" => Condition.Good,
        "Heavily Played" => Condition.Played,
        _ => Condition.Unknown
    };

    private static Language GetLanguage(string value) => value switch
    {
        "English" => Language.English,
        "German" => Language.German,
        "French" => Language.French,
        "Japanese" => Language.Japanese,
        "Spanish" => Language.Spanish,
        "Chinese Traditional" => Language.TraditionalChinese,
        "Portuguese" => Language.Portuguese,
        "Italian" => Language.Italian,
        "Chinese Simplified" => Language.SimplifiedChinese,
        "Russian" => Language.Russian,
        "Korean" => Language.Korean,
        _ => Language.Unknown
    };
}
