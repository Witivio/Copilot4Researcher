using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using System.Globalization;

public class CommaDecimalConverter : DecimalConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrEmpty(text)) return 0;
        return decimal.Parse(text.Replace(',', '.'), CultureInfo.InvariantCulture);
    }
} 