using Informagator.Contracts;
using Informagator.Contracts.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Informagator.Manager.Controls
{
    public class StageTypePicker : TypePicker<IProcessingStage>
    {
        static StageTypePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StageTypePicker), new FrameworkPropertyMetadata(typeof(StageTypePicker)));
            FocusableProperty.OverrideMetadata(typeof(StageTypePicker), new FrameworkPropertyMetadata(false));
        }

        protected override void OnSelectedAssemblyIdChanged()
        {
            base.OnSelectedAssemblyIdChanged();
        }
    }
}
