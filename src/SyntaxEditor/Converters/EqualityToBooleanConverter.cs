using System;
using System.Globalization;
using System.Windows.Data;

namespace SyntaxEditor.Converters
{
    /// <summary>
    /// Converter that checks equality between value and parameter.
    /// </summary>
    internal sealed class EqualityToBooleanConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(value, parameter);
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return b ? parameter : null;

            throw new ArgumentException(
                $"{nameof(IntegerToDoubleConverter)} back conversion must take a bool in parameter.",
                nameof(value));
        }
    }
}