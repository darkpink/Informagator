using Acadian.Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Acadian.Informagator.Manager.Controls
{
    public class WorkerTypePicker : TypePicker<IInformagatorWorker>
    {
        static WorkerTypePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WorkerTypePicker), new FrameworkPropertyMetadata(typeof(WorkerTypePicker)));
            FocusableProperty.OverrideMetadata(typeof(WorkerTypePicker), new FrameworkPropertyMetadata(false));
        }

    }
}
