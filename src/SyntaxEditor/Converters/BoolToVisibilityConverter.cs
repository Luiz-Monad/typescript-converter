using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SyntaxEditor.Converters
{
    /// <summary>
    /// Converter from <see cref="bool"/> to <see cref="Visibility"/>.
    /// </summary>
    internal sealed class BoolToVisibilityConverter : IValueConverter
    {
        #region IValueConverter

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return b ? Visibility.Visible : Visibility.Collapsed;

            throw new ArgumentException(
                $"{nameof(BoolToVisibilityConverter)} must take a boolean in parameter.",
                nameof(value));
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility v)
                return v == Visibility.Visible;

            throw new ArgumentException(
                $"{nameof(BoolToVisibilityConverter)} back conversion must take a {nameof(Visibility)} in parameter.",
                nameof(value));
        }

        #endregion
    }
}