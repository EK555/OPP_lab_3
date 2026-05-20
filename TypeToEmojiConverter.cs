using System.Globalization;

namespace FinanceTrackerApp
{
    public class TypeToEmojiConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string type)
            {
                return type == "Income" ? "💰" : "💸";
            }
            return "💰";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}