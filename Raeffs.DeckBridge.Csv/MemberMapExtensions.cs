using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Raeffs.DeckBridge.Csv;

public static class MemberMapExtensions
{
    public static MemberMap<TClass, bool> ConfigureBoolean<TClass>(this MemberMap<TClass, bool> memberMap, string trueValue, string falseValue)
    {
        memberMap
            .TypeConverter<BooleanConverter>()
            .TypeConverterOption.BooleanValues(true, true, trueValue)
            .TypeConverterOption.BooleanValues(false, true, falseValue);

        return memberMap;
    }

    public static MemberMap<TClass, DateTime?> ConfigureDateTime<TClass>(this MemberMap<TClass, DateTime?> memberMap, string format)
    {
        memberMap
            .TypeConverter<DateTimeConverter>()
            .TypeConverterOption.Format(format);

        memberMap.Data.Default = null;
        memberMap.Data.IsDefaultSet = true;

        return memberMap;
    }
}
