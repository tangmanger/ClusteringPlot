using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ClusteringPlot.Converters
{
    public class ScrollBarMaximumConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double outBorderValue = (double)values[0];
            double innerValue = (double)values[1];
            if (values.Length == 3)
                innerValue = (double)values[1] > (double)values[2] ? (double)values[1] : (double)values[2];
            return innerValue - outBorderValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
