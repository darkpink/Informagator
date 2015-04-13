using Informagator.Contracts.Stages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public static DependencyProperty SuppressParentErrorHandlersProperty = DependencyProperty.Register("SuppressParentErrorHandlers", typeof(bool), typeof(StageEditor), new PropertyMetadata(false, new PropertyChangedCallback(SuppressParentErrorHandlersChanged)));
        public bool SuppressParentErrorHandlers
        {
            get
            {
                return (bool)GetValue(SuppressParentErrorHandlersProperty);
            }
            set
            {
                SetValue(SuppressParentErrorHandlersProperty, value);
            }
        }
        public static void SuppressParentErrorHandlersChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            StageEditor editor = sender as StageEditor;
            if (editor != null)
            {
                editor.SuppressParentErrorHandlersChanged();
            }
        }
        protected virtual void SuppressParentErrorHandlersChanged()
        {
        }

        public static DependencyProperty ErrorHandlerIdsProperty = DependencyProperty.Register("ErrorHandlerIds", typeof(ObservableCollection<long?>), typeof(StageEditor), new PropertyMetadata(new PropertyChangedCallback(ErrorHandlersChanged)));

        public ObservableCollection<long?> ErrorHandlerIds
        {
            get
            {
                return (ObservableCollection<long?>)GetValue(ErrorHandlerIdsProperty);
            }
            set
            {
                SetValue(ErrorHandlerIdsProperty, value);
            }
        }

        public static void ErrorHandlersChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            StageEditor editor = sender as StageEditor;
            if (editor != null)
            {
                editor.ErrorHandlersChanged();
            }
        }
        protected virtual void ErrorHandlersChanged()
        {
        }

    }
}
