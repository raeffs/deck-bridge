using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Moxfield;

/// <summary>
/// https://www.moxfield.com/help/importing-collection
/// </summary>
internal class MoxfieldCardMap : ClassMap<Card>
{
    public MoxfieldCardMap()
    {
        Map(x => x.Quantity).Name("Count");
        Map(x => x.Name).Name("Name");
        Map(x => x.SetCode).Name("Edition");
        Map(x => x.Condition).Name("Condition").TypeConverter<ConditionConverter>();
        Map(x => x.Language).Name("Language").TypeConverter<LanguageConverter>();
        Map(x => x.IsFoil).Name("Foil").TypeConverter<FoilConverter>();
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
            Language.English => "English",
            Language.German => "German",
            Language.French => "French",
            Language.Spanish => "Spanish",
            Language.Italian => "Italian",
            Language.SimplifiedChinese => "Simplified Chinese",
            Language.TraditionalChinese => "Traditional Chinese",
            Language.Japanese => "Japanese",
            Language.Portuguese => "Portuguese",
            Language.Korean => "Korean",
            Language.Russian => "Russian",
            Language.Phyrexian => "Phyrexian",
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
            Condition.Mint => "M",
            Condition.NearMint => "NM",
            Condition.Excellent => "LP",
            Condition.Good or Condition.LightPlayed => "MP",
            Condition.Played => "HP",
            Condition.Poor => "DM",
            _ => string.Empty
        };
    }

    public class FoilConverter : DefaultTypeConverter
    {
        public override string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
        {
            return value is bool booleanValue
                ? booleanValue ? "foil" : string.Empty
                : base.ConvertToString(value, row, memberMapData);
        }
    }
}
