﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace O2ArchiveReader.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool isVisible)
                ? (isVisible ? Visibility.Visible : Visibility.Collapsed)
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
