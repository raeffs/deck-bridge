using CsvHelper.Configuration;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Csv;

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
        Map(x => x.IsFoil).Name("Premium").ConfigureBoolean("Yes", "No");
    }
}
