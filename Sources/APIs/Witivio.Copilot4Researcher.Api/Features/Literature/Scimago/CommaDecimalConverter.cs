using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using System.Globalization;

/// <summary>
/// Custom converter for CsvHelper to handle decimal values that use comma as decimal separator.
/// Converts comma-formatted decimal strings (e.g., "1,23") to decimal values by replacing the comma with a period.
/// </summary>
public class CommaDecimalConverter : DecimalConverter
{
    /// <summary>
    /// Converts a string value from CSV to a decimal, handling comma decimal separators.
    /// </summary>
    /// <param name="text">The string value from the CSV file</param>
    /// <param name="row">The current CSV row being processed</param>
    /// <param name="memberMapData">Mapping information for the current field</param>
    /// <returns>Decimal value, or 0 if the input is empty</returns>
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrEmpty(text)) return 0;
        return decimal.Parse(text.Replace(',', '.'), CultureInfo.InvariantCulture);
    }
} 