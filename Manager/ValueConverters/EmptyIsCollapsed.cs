using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Collections;
using System.Windows.Markup;

namespace Acadian.Informagator.Manager.ValueConverters
{

    public class EmptyIsCollapsed : MarkupExtension, IValueConverter 
    {
        public EmptyIsCollapsed()
            :base()
        {
        }

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture) 
        {
            Visibility result;

            if (value is bool)
            {
                bool show = (bool)value;
                result = show ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (value is string)
            {
                result = String.IsNullOrEmpty(value as string) ? Visibility.Collapsed : Visibility.Visible;
            }
            else if (value is ICollection)
            {
                ICollection collection = value as ICollection;
                result = (collection == null || collection.Count == 0) ? Visibility.Collapsed : Visibility.Visible;
            }
            else if (value == null)
            {
                result = Visibility.Collapsed;
            }
            else
            {
                result = Visibility.Visible;
            }
            
            return result;
        } 
 
        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture) 
        {
            throw new InvalidOperationException("ConvertBack not supported on this value converter");
        }

        private static EmptyIsCollapsed instance; 

        public override object ProvideValue(IServiceProvider serviceProvider) 
        {
            if (instance == null)
            {
                instance = new EmptyIsCollapsed();
            }
            
            return instance; 
        } 
    } 
 }
