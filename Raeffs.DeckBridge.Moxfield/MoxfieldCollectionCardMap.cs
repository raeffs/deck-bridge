using CsvHelper.Configuration;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Csv;

namespace Raeffs.DeckBridge.Moxfield;

/// <summary>
/// https://www.moxfield.com/help/importing-collection
/// </summary>
internal class MoxfieldCollectionCardMap : ClassMap<Card>
{
    public MoxfieldCollectionCardMap()
    {
        Map(x => x.Quantity).Name("Count");
        Map(x => x.Name).Name("Name");
        Map(x => x.SetCode).Name("Edition");
        Map(x => x.Condition).Name("Condition").TypeConverter<ConditionConverter>();
        Map(x => x.Language).Name("Language").TypeConverter<LanguageConverter>();
        Map(x => x.IsFoil).Name("Foil").ConfigureBoolean("foil", string.Empty);
    }

    public class LanguageConverter : TypedConverter<Language>
    {
        protected override Language ConvertFromString(string text) => text switch
        {
            "English" => Language.English,
            "German" => Language.German,
            "French" => Language.French,
            "Spanish" => Language.Spanish,
            "Italian" => Language.Italian,
            "Simplified Chinese" => Language.SimplifiedChinese,
            "Traditional Chinese" => Language.TraditionalChinese,
            "Japanese" => Language.Japanese,
            "Portuguese" => Language.Portuguese,
            "Korean" => Language.Korean,
            "Russian" => Language.Russian,
            "Phyrexian" => Language.Phyrexian,
            _ => Language.Unknown
        };

        protected override string ConvertToString(Language value) => value switch
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

    public class ConditionConverter : TypedConverter<Condition>
    {
        protected override Condition ConvertFromString(string text) => text switch
        {
            "M" => Condition.Mint,
            "NM" => Condition.NearMint,
            "LP" => Condition.Excellent,
            "MP" => Condition.Good,
            "HP" => Condition.Played,
            "DM" => Condition.Poor,
            _ => Condition.Unknown
        };

        protected override string ConvertToString(Condition value) => value switch
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
}
