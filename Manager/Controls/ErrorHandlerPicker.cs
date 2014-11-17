using Acadian.Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Acadian.Informagator.Manager.Controls
{
    public class ErrorHandlerPicker : TypePicker<IMessageErrorHandler>
    {
        static ErrorHandlerPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ErrorHandlerPicker), new FrameworkPropertyMetadata(typeof(ErrorHandlerPicker)));
            FocusableProperty.OverrideMetadata(typeof(ErrorHandlerPicker), new FrameworkPropertyMetadata(false));
        }
    }
}
