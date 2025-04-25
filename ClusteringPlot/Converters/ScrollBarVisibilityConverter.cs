using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace ClusteringPlot.Converters
{
    public class ScrollBarVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double outBorderValue = (double)values[0];
            double innerValue = (double)values[1];
            if (values.Length == 3)
                innerValue = (double)values[1] > (double)values[2] ? (double)values[1] : (double)values[2];
            return innerValue > outBorderValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
