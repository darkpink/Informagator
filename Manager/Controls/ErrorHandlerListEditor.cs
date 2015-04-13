using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Informagator.Manager.Controls
{
    public class ErrorHandlerListEditor : Control
    {
        static ErrorHandlerListEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ErrorHandlerListEditor), new FrameworkPropertyMetadata(typeof(ErrorHandlerListEditor)));
            FocusableProperty.OverrideMetadata(typeof(ErrorHandlerListEditor), new FrameworkPropertyMetadata(false));
        }


        public ErrorHandlerListEditor()
        {
        }

        public static DependencyProperty ErrorHandlerIdsProperty = DependencyProperty.Register("ErrorHandlerIds", typeof(ObservableCollection<long?>), typeof(ErrorHandlerListEditor), new PropertyMetadata(new PropertyChangedCallback(ErrorHandlersChanged)));
        
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
            ErrorHandlerListEditor editor = sender as ErrorHandlerListEditor;
            if (editor != null)
            {
                editor.ErrorHandlersChanged();
            }
        }
        protected virtual void ErrorHandlersChanged()
        {
            if (ErrorHandlerIds != null)
            {
                ErrorHandlerIds.CollectionChanged += ErrorHandlers_CollectionChanged;
                BuildErrorHandlers();
            }
        }

        private void BuildErrorHandlers()
        {
            if (PART_PrimaryGrid != null)
            {
                PART_PrimaryGrid.Children.Clear();
                PART_PrimaryGrid.RowDefinitions.Clear();

                int newNumberOfRowDefinitions = ErrorHandlerIds == null ? 1 : ErrorHandlerIds.Count + 1;
                Enumerable.Range(0, newNumberOfRowDefinitions).ToList().ForEach(
                    n => PART_PrimaryGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto }));

                for (int index = 0; index < (ErrorHandlerIds == null ? 0 : ErrorHandlerIds.Count); index++)
                {
                    ErrorHandlerPicker editor = new ErrorHandlerPicker();
                    Binding entityIdBinding = new Binding();
                    entityIdBinding.Path = new PropertyPath(String.Format("[{0}]", index));
                    entityIdBinding.Source = ErrorHandlerIds;
                    entityIdBinding.Mode = BindingMode.TwoWay;
                    entityIdBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    BindingOperations.SetBinding(editor, ErrorHandlerPicker.EntityIdProperty, entityIdBinding);
                    Grid.SetRow(editor, index);
                    PART_PrimaryGrid.Children.Add(editor);

                    Button removeButton = new Button();
                    removeButton.Style = PART_PrimaryGrid.Resources["RemoveErrorHandler"] as Style;
                    removeButton.DataContext = ErrorHandlerIds[index];
                    removeButton.Click += RemoveButton_Click;
                    Grid.SetRow(removeButton, index);
                    Grid.SetColumn(removeButton, 1);
                    PART_PrimaryGrid.Children.Add(removeButton);
                }

                Button addButton = new Button();
                addButton.Style = PART_PrimaryGrid.Resources["AddErrorHandler"] as Style;
                addButton.Click += AddButton_Click;
                Grid.SetRow(addButton, newNumberOfRowDefinitions);
                //Grid.SetColumn(addButton, 1);
                addButton.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                PART_PrimaryGrid.Children.Add(addButton);
            }
        }

        protected void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            long? stg = (sender as Button).DataContext as long?;
            ErrorHandlerIds.Remove(stg);
        }

        protected void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (ErrorHandlerIds != null)
            {
                ErrorHandlerIds.Add(null);
            }
        }

        public void ErrorHandlers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (PART_PrimaryGrid != null)
            {
                BuildErrorHandlers();
            }
        }

        protected Grid PART_PrimaryGrid { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_PrimaryGrid = GetTemplateChild("PART_PrimaryGrid") as Grid;
            BuildErrorHandlers();
        }
    }
}
