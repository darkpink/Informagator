using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Informagator.Manager.Controls
{
    public class WorkerEditor : ConfigurableTypeEditor<WorkerTypePicker, IWorker>
    {
        static WorkerEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WorkerEditor), new FrameworkPropertyMetadata(typeof(WorkerEditor)));
            FocusableProperty.OverrideMetadata(typeof(WorkerEditor), new FrameworkPropertyMetadata(false));
        }

        protected override string TypeCaption
        {
            get { return "Worker"; }
        }

        public static DependencyProperty MachineIdProperty = DependencyProperty.Register("MachineId", typeof(long?), typeof(WorkerEditor), new PropertyMetadata(new PropertyChangedCallback(MachineIdChanged)));
        public long? MachineId
        {
            get
            {
                return (long?)GetValue(MachineIdProperty);
            }
            set
            {
                SetValue(MachineIdProperty, value);
            }
        }

        public static void MachineIdChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            WorkerEditor picker = sender as WorkerEditor;
            if (picker != null)
            {
                picker.OnMachineIdChanged();
            }
        }

        protected virtual void OnMachineIdChanged()
        {
        }

        public static DependencyProperty AutoStartProperty = DependencyProperty.Register("AutoStart", typeof(bool), typeof(WorkerEditor), new PropertyMetadata(new PropertyChangedCallback(AutoStartChanged)));
        public bool AutoStart
        {
            get
            {
                return (bool)GetValue(AutoStartProperty);
            }
            set
            {
                SetValue(AutoStartProperty, value);
            }
        }

        public static void AutoStartChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            WorkerEditor picker = sender as WorkerEditor;
            if (picker != null)
            {
                picker.OnAutoStartChanged();
            }
        }

        protected virtual void OnAutoStartChanged()
        {
        }

        public static DependencyProperty OverrideMachineErrorHandlersProperty = DependencyProperty.Register("OverrideMachineErrorHandlers", typeof(bool), typeof(WorkerEditor), new PropertyMetadata(new PropertyChangedCallback(OverrideMachineErrorHandlersChanged)));
        public bool OverrideMachineErrorHandlers
        {
            get
            {
                return (bool)GetValue(OverrideMachineErrorHandlersProperty);
            }
            set
            {
                SetValue(OverrideMachineErrorHandlersProperty, value);
            }
        }

        public static void OverrideMachineErrorHandlersChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            WorkerEditor picker = sender as WorkerEditor;
            if (picker != null)
            {
                picker.OnOverrideMachineErrorHandlersChanged();
            }
        }

        protected virtual void OnOverrideMachineErrorHandlersChanged()
        {
        }
    }
}
