using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Acadian.Informagator.Manager.ValueConverters
{
    public class PassthroughConverter : MarkupExtension, IValueConverter
    {
        public PassthroughConverter()
        {

        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        private PassthroughConverter _instance;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_instance == null)
            {
                _instance = new PassthroughConverter();
            }
            
            return _instance;
        }
    }
}
