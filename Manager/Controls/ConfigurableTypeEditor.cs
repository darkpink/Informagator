using Informagator.Contracts.Configuration;
using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Informagator.Manager.Controls
{
    public abstract class ConfigurableTypeEditor<TPickerType, TType> : Control
        where TPickerType : TypePicker<TType>, new()
    {
        protected abstract string TypeCaption { get; }

        public static DependencyProperty EntityNameProperty = DependencyProperty.Register("EntityName", typeof(string), typeof(ConfigurableTypeEditor<TPickerType, TType>), new PropertyMetadata(new PropertyChangedCallback(NameChanged)));
        public string EntityName
        {
            get
            {
                return (string)GetValue(EntityNameProperty);
            }
            set
            {
                SetValue(EntityNameProperty, value);
            }
        }

        public static void NameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ConfigurableTypeEditor<TPickerType, TType> picker = sender as ConfigurableTypeEditor<TPickerType, TType>;
            if (picker != null)
            {
                picker.OnNameChanged();
            }
        }

        protected virtual void OnNameChanged()
        {
        }

        public static DependencyProperty AssemblyIdProperty = DependencyProperty.Register("AssemblyId", typeof(long?), typeof(ConfigurableTypeEditor<TPickerType, TType>), new PropertyMetadata(new PropertyChangedCallback(AssemblyIdChanged)));
        public long? AssemblyId
        {
            get
            {
                return (long?)GetValue(AssemblyIdProperty);
            }
            set
            {
                SetValue(AssemblyIdProperty, value);
            }
        }

        public static void AssemblyIdChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ConfigurableTypeEditor<TPickerType, TType> picker = sender as ConfigurableTypeEditor<TPickerType, TType>;
            if (picker != null)
            {
                picker.OnAssemblyIdChanged();
            }
        }

        protected virtual void OnAssemblyIdChanged()
        {
        }


        public static DependencyProperty EntityTypeProperty = DependencyProperty.Register("EntityType", typeof(string), typeof(ConfigurableTypeEditor<TPickerType, TType>), new PropertyMetadata(new PropertyChangedCallback(EntityTypeChanged)));
        public string EntityType
        {
            get
            {
                return (string)GetValue(EntityTypeProperty);
            }
            set
            {
                SetValue(EntityTypeProperty, value);
            }
        }

        public static void EntityTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ConfigurableTypeEditor<TPickerType, TType> picker = sender as ConfigurableTypeEditor<TPickerType, TType>;
            if (picker != null)
            {
                picker.OnEntityTypeChanged();
            }
        }

        protected virtual void OnEntityTypeChanged()
        {
            BuildParameterGrid();
        }

        public static DependencyProperty ParametersProperty = DependencyProperty.Register("Parameters", typeof(ObservableCollection<Parameter>), typeof(ConfigurableTypeEditor<TPickerType, TType>), new PropertyMetadata(new PropertyChangedCallback(ParametersChanged)));

        public ObservableCollection<Parameter> Parameters
        {
            get
            {
                return (ObservableCollection<Parameter>)GetValue(ParametersProperty);
            }
            set
            {
                SetValue(ParametersProperty, value);
            }
        }

        public static void ParametersChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ConfigurableTypeEditor<TPickerType, TType> picker = sender as ConfigurableTypeEditor<TPickerType, TType>;
            if (picker != null)
            {
                picker.OnParametersChanged();
            }
        }

        protected virtual void OnParametersChanged()
        {
            BuildParameterGrid();
        }

        private List<Parameter> GetConfigurationParametersForType(long? assemblyId, string type)
        {
            List<Parameter> result = new List<Parameter>();

            if (assemblyId != null)
            {
                using (ConfigurationEntities entities = new ConfigurationEntities())
                {
                    byte[] assemblyBinary = entities.Assemblies
                                            .Where(av => av.Id == assemblyId)
                                            .Select(av => av.Executable)
                                            .SingleOrDefault();
                    if (assemblyBinary != null && !String.IsNullOrWhiteSpace(type))
                    {
                        AppDomain tempDomain = AppDomain.CreateDomain("tempDomain");
                        AssemblyInspector inspector = tempDomain.CreateInstanceAndUnwrap(this.GetType().Assembly.FullName, typeof(AssemblyInspector).FullName) as AssemblyInspector;
                        result = inspector.Inspect(assemblyBinary, type);
                        AppDomain.Unload(tempDomain);
                    }
                }
            }
            return result;
        }

        private void BuildParameterGrid()
        {
            if (Parameters != null && PART_ParametersGrid != null)
            {
                List<Parameter> configParams = GetConfigurationParametersForType(AssemblyId, EntityType);
                PART_ParametersGrid.Children.Clear();
                PART_ParametersGrid.RowDefinitions.Clear();
                Enumerable.Range(0, configParams.Count).ToList().ForEach(n => PART_ParametersGrid.RowDefinitions.Add(new RowDefinition()));

                int rowNumber = 0;
                foreach (var parameter in configParams)
                {
                    Control editControl;
                    if (!Parameters.Any(p => p.Name == parameter.Name))
                    {
                        Parameters.Add(new Parameter() { Name = parameter.Name });
                    }

                    var uiParameter = Parameters.Single(p => p.Name == parameter.Name);
                    Binding editBinding = new Binding();
                    editBinding.Source = uiParameter;
                    editBinding.Path = new PropertyPath("Value");
                    editBinding.Mode = BindingMode.TwoWay;
                    editBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

                    if (parameter.PropertyType == typeof(bool))
                    {
                        editControl = new CheckBox();
                        BindingOperations.SetBinding(editControl, CheckBox.IsCheckedProperty, editBinding);
                    }
                    else
                    {
                        editControl = new TextBox();
                        BindingOperations.SetBinding(editControl, TextBox.TextProperty, editBinding);
                    }

                    TextBlock caption = new TextBlock() { HorizontalAlignment = System.Windows.HorizontalAlignment.Right, Text = parameter.DisplayName };

                    Grid.SetColumn(editControl, 1);
                    Grid.SetRow(caption, rowNumber);
                    Grid.SetRow(editControl, rowNumber);
                    PART_ParametersGrid.Children.Add(caption);
                    PART_ParametersGrid.Children.Add(editControl);
                    rowNumber++;
                }

                Parameters.Where(uiParam => !configParams.Any(configParam => uiParam.Name == configParam.Name))
                          .ToList()
                          .ForEach(uiParam => Parameters.Remove(uiParam));
            }
        }

        protected TPickerType PART_TypePicker { get; set; }
        protected Grid PART_ParametersGrid { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Grid part_EditorGrid = GetTemplateChild("PART_EditorGrid") as Grid;
            PART_ParametersGrid = GetTemplateChild("PART_ParametersGrid") as Grid;

            AddTypePickerToGrid(part_EditorGrid);
            BuildParameterGrid();
        }

        protected void AddTypePickerToGrid(Grid editorGrid)
        {
            PART_TypePicker = new TPickerType();
            Grid.SetRow(PART_TypePicker, 1);
            Grid.SetColumnSpan(PART_TypePicker, 2);
            PART_TypePicker.TypeCaption = TypeCaption;
            PART_TypePicker.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

            Binding selectedAssemblyIdBinding = new Binding();
            selectedAssemblyIdBinding.Source = this;
            selectedAssemblyIdBinding.Path = new PropertyPath("AssemblyId");
            selectedAssemblyIdBinding.Mode = BindingMode.TwoWay;
            selectedAssemblyIdBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(PART_TypePicker, TypePicker<TType>.SelectedAssemblyIdProperty, selectedAssemblyIdBinding);

            Binding selectedTypeBinding = new Binding();
            selectedTypeBinding.Source = this;
            selectedTypeBinding.Path = new PropertyPath("EntityType");
            selectedTypeBinding.Mode = BindingMode.TwoWay;
            selectedTypeBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(PART_TypePicker, TypePicker<TType>.SelectedTypeProperty, selectedTypeBinding);

            editorGrid.Children.Add(PART_TypePicker);
        }
    }
}
