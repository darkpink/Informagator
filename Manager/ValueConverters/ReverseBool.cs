using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace Informagator.Manager.ValueConverters
{
    public class ReverseBool : MarkupExtension, IValueConverter
    {
        public ReverseBool()
            : base()
        {
        }

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool result = (value as bool?) != true;
            return result;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool result = (value as bool?) != true;
            return result;
        }

        private static ReverseBool instance;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (instance == null)
            {
                instance = new ReverseBool();
            }

            return instance;
        }
    }
}
