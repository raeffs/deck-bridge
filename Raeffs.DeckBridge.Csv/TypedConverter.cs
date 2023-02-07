using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Raeffs.DeckBridge.Csv;

public abstract class TypedConverter<T> : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        return ConvertFromString(text ?? string.Empty);
    }

    protected abstract T ConvertFromString(string text);

    public override string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
    {
        return value is T typedValue
            ? ConvertToString(typedValue)
            : base.ConvertToString(value, row, memberMapData);
    }

    protected abstract string ConvertToString(T value);
}
