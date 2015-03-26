using Informagator.Contracts.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Informagator.Manager.Controls
{
    public class StageEditor: ConfigurableTypeEditor<StageTypePicker, IProcessingStage>
    {
        static StageEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StageEditor), new FrameworkPropertyMetadata(typeof(StageEditor)));
            FocusableProperty.OverrideMetadata(typeof(StageEditor), new FrameworkPropertyMetadata(false));
        }

        protected override string TypeCaption
        {
            get { return "Stage"; }
        }

        public static DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(StageEditor), new PropertyMetadata(false, new PropertyChangedCallback(IsExpandedChanged)));
        public bool IsExpanded
        {
            get
            {
                return (bool)GetValue(IsExpandedProperty);
            }
            set
            {
                SetValue(IsExpandedProperty, value);
            }
        }
        public static void IsExpandedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            StageEditor editor = sender as StageEditor;
            if (editor != null)
            {
                editor.IsExpandedChanged();
            }
        }
        protected virtual void IsExpandedChanged()
        {
        }

        protected Border PART_IsExpandedClickBorder { get; set; }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_IsExpandedClickBorder = GetTemplateChild("PART_IsExpandedClickBorder") as Border;
            PART_IsExpandedClickBorder.MouseDown += delegate(object sender, MouseButtonEventArgs args) { IsExpanded = !IsExpanded; };
        }

    }
}
