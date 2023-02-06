using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.MagicOnline;

/// <summary>
/// https://www.mtggoldfish.com/help/import_formats
/// </summary>
internal class MagicOnlineCardMap : ClassMap<Card>
{
    public MagicOnlineCardMap()
    {
        Map(x => x.Name).Name("Card Name");
        Map(x => x.Quantity).Name("Quantity");
        Map(x => x.Rarity).Name("Rarity");
        Map(x => x.SetCode).Name("Set");
        Map(x => x.CollectorNumber).Name("Collector #");
        Map(x => x.IsFoil).Name("Premium").TypeConverter<FoilConverter>();
    }

    public class FoilConverter : DefaultTypeConverter
    {
        public override string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
        {
            return value is bool booleanValue
                ? booleanValue ? "Yes" : "No"
                : base.ConvertToString(value, row, memberMapData);
        }
    }
}
