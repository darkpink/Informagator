using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Informagator.Manager.Controls
{
    public class ErrorHandlerTypePicker : TypePicker<IMessageErrorHandler>
    {
        static ErrorHandlerTypePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ErrorHandlerTypePicker), new FrameworkPropertyMetadata(typeof(ErrorHandlerTypePicker)));
            FocusableProperty.OverrideMetadata(typeof(ErrorHandlerTypePicker), new FrameworkPropertyMetadata(false));
        }
    }
}
