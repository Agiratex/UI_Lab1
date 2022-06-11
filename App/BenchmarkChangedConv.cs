using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace App
{
    public class BenchmarkChangedConv : IValueConverter
    {
        public object Convert(object value, Type target_type, object param, CultureInfo culture)
        {
            try
            {
                bool val = (bool)value;
                if (val)
                    return "Collection was changed";
                else
                    return "Collection wasn't changed";
            }
            catch (Exception err)
            {
                MessageBox.Show($"Get error: {err.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "Error";
            }
        }

        public object ConvertBack(object value, Type target_type, object param, CultureInfo culture)
        {
            return false;
        }
    }
}
