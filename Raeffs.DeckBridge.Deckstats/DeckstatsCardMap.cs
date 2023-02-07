using CsvHelper.Configuration;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Csv;

namespace Raeffs.DeckBridge.Deckstats;

/// <summary>
/// https://deckstats.net/collection/43995/import/?lng=en
/// 
/// card_name: The card name(required) [can also be called Name or Card]
/// amount: The amount[can also be called Count, Reg Qty or Qty]
/// amount_foil: The amount of foil copies(if not using the is_foil column) [can also be called Foil Qty]
/// set_name: Three-letter abbreviation(eg.LEA, UST) or name of the set[can also be called set_code, Printing or Edition]
/// collector_number: Number of the exact printing within the set(only required if there are multiple versions of the card) [can also be called Card Number]
/// language: Two-letter language code(eg.EN, DE, ES) or name[can also be called Languange]
/// condition: One/two-letter condition code(eg.NM, MP, DM) or name
/// added: Date the card was added to your collection.ISO 8601 date format (YYYY-MM-DD) suggested, standard US format(m/d/y) should also work.
/// is_foil: Should contain a 1 if the card is foil, empty otherwise[can also be called Foil]
/// is_pinned: Should contain a 1 if the card is pinned, empty otherwise
/// is_signed: Should contain a 1 if the card is signed, empty otherwise
/// comment: A comment/notes about the card[can also be called Notes]
/// </summary>
internal class DeckstatsCardMap : ClassMap<Card>
{
    private const string TrueValue = "1";
    private const string FalseValue = "0";

    private const string DateTimeFormat = "yyyy-MM-dd";

    public DeckstatsCardMap()
    {
        Map(x => x.Name).Name("card_name");
        Map(x => x.Quantity).Name("amount");
        Map(x => x.SetCode).Name("set_name");
        Map(x => x.CollectorNumber).Name("collector_number");
        Map(x => x.Language).Name("language").TypeConverter<LanguageConverter>();
        Map(x => x.Condition).Name("condition").TypeConverter<ConditionConverter>();
        Map(x => x.Added).Name("added").ConfigureDateTime(DateTimeFormat);
        Map(x => x.IsFoil).Name("is_foil").ConfigureBoolean(TrueValue, FalseValue);
        Map(x => x.IsPinned).Name("is_pinned").ConfigureBoolean(TrueValue, FalseValue);
        Map(x => x.IsSigned).Name("is_signed").ConfigureBoolean(TrueValue, FalseValue);
        Map(x => x.Comment).Name("comment");
    }

    public class LanguageConverter : TypedConverter<Language>
    {
        protected override Language ConvertFromString(string text) => text switch
        {
            "en" => Language.English,
            "de" => Language.German,
            "fr" => Language.French,
            "es" => Language.Spanish,
            "it" => Language.Italian,
            "cn" => Language.SimplifiedChinese,
            "jp" => Language.Japanese,
            "pt" => Language.Portuguese,
            "kr" => Language.Korean,
            "ru" => Language.Russian,
            "ph" => Language.Phyrexian,
            _ => Language.Unknown
        };

        protected override string ConvertToString(Language value) => value switch
        {
            Language.English => "en",
            Language.German => "de",
            Language.French => "fr",
            Language.Spanish => "es",
            Language.Italian => "it",
            Language.SimplifiedChinese or Language.TraditionalChinese => "cn",
            Language.Japanese => "jp",
            Language.Portuguese => "pt",
            Language.Korean => "kr",
            Language.Russian => "ru",
            Language.Phyrexian => "ph",
            _ => string.Empty
        };
    }

    public class ConditionConverter : TypedConverter<Condition>
    {
        protected override Condition ConvertFromString(string text) => text switch
        {
            "NM" => Condition.NearMint,
            "LP" => Condition.Excellent,
            "MP" => Condition.Good,
            "HP" => Condition.Played,
            "DM" => Condition.Poor,
            _ => Condition.Unknown
        };

        protected override string ConvertToString(Condition value) => value switch
        {
            Condition.Mint or Condition.NearMint => "NM",
            Condition.Excellent => "LP",
            Condition.Good or Condition.LightPlayed => "MP",
            Condition.Played => "HP",
            Condition.Poor => "DM",
            _ => string.Empty
        };
    }
}
