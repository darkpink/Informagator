using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Informagator.Manager.Controls
{
    public class ErrorHandlerEditor :ConfigurableTypeEditor<ErrorHandlerPicker,IMessageErrorHandler>
    {
        static ErrorHandlerEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ErrorHandlerEditor), new FrameworkPropertyMetadata(typeof(ErrorHandlerEditor)));
            FocusableProperty.OverrideMetadata(typeof(ErrorHandlerEditor), new FrameworkPropertyMetadata(false));
        }

        protected override string TypeCaption
        {
            get { return "Error Handler"; }
        }
    }
}
