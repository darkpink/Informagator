using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Acadian.Informagator.Manager.Controls.StageEditor
{
    public class WorkerStageEditor : Control
    {
        static WorkerStageEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WorkerStageEditor), new FrameworkPropertyMetadata(typeof(WorkerStageEditor)));
            FocusableProperty.OverrideMetadata(typeof(WorkerStageEditor), new FrameworkPropertyMetadata(false));
        }


    }
}
