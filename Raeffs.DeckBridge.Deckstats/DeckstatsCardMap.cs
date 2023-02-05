using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Raeffs.DeckBridge.Common;

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
    public DeckstatsCardMap()
    {
        Map(x => x.Name).Name("card_name");
        Map(x => x.Quantity).Name("amount");
        Map(x => x.SetCode).Name("set_name");
        Map(x => x.CollectorNumber).Name("collector_number");
        Map(x => x.Language).Name("language").TypeConverter<LanguageConverter>();
        Map(x => x.Condition).Name("condition").TypeConverter<ConditionConverter>();
        Map(x => x.Added).Name("added").TypeConverter<DateTimeConverter>();
        Map(x => x.IsFoil).Name("is_foil").TypeConverter<BooleanConverter>();
        Map(x => x.IsPinned).Name("is_pinned").TypeConverter<BooleanConverter>();
        Map(x => x.IsSigned).Name("is_signed").TypeConverter<BooleanConverter>();
        Map(x => x.Comment).Name("comment");
    }

    public class LanguageConverter : DefaultTypeConverter
    {
        public override string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
        {
            return value is Language languageValue
                ? GetLanguage(languageValue)
                : base.ConvertToString(value, row, memberMapData);
        }

        private static string GetLanguage(Language value) => value switch
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

    public class ConditionConverter : DefaultTypeConverter
    {
        public override string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
        {
            return value is Condition conditionValue
                ? GetCondition(conditionValue)
                : base.ConvertToString(value, row, memberMapData);
        }

        private static string GetCondition(Condition value) => value switch
        {
            Condition.Mint or Condition.NearMint => "NM",
            Condition.Excellent or Condition.Good => "LP",
            Condition.LightPlayed => "MP",
            Condition.Played => "HP",
            Condition.Poor => "DM",
            _ => string.Empty
        };
    }

    public class DateTimeConverter : DefaultTypeConverter
    {
        public override string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
        {
            return value is DateTime dateTimeValue
                ? dateTimeValue.ToString("yyyy-MM-dd")
                : base.ConvertToString(value, row, memberMapData);
        }
    }

    public class BooleanConverter : DefaultTypeConverter
    {
        public override string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
        {
            return value is bool booleanValue
                ? booleanValue ? "1" : "0"
                : base.ConvertToString(value, row, memberMapData);
        }
    }
}
