using Informagator.Contracts.Stages;
using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Informagator.Manager.Controls.StageEditor
{
    /// <summary>
    /// Interaction logic for StageEdit.xaml
    /// </summary>
    public partial class StageEdit : UserControl
    {
        public StageEdit()
        {
            InitializeComponent();
            StageParameters = new ObservableCollection<StageParameter>();

            PART_IsExpandedClickBorder.MouseDown += delegate(object sender, MouseButtonEventArgs args) { IsExpanded = !IsExpanded; };
            PART_StageTypePicker.SelectedTypeChanged += PART_StageTypePicker_SelectedTypeChanged;
        }

        public static DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(StageEdit), new PropertyMetadata(false, new PropertyChangedCallback(IsExpandedChanged)));
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
            StageEdit editor = sender as StageEdit;
            if (editor != null)
            {
                editor.IsExpandedChanged();
            }
        }
        protected virtual void IsExpandedChanged()
        {
        }

        public static DependencyProperty SelectedConfigurationProperty = DependencyProperty.Register("SelectedConfiguration", typeof(string), typeof(StageEdit), new PropertyMetadata(new PropertyChangedCallback(SelectedConfigurationChanged)));
        public string SelectedConfiguration
        {
            get
            {
                return (string)GetValue(SelectedConfigurationProperty);
            }
            set
            {
                SetValue(SelectedConfigurationProperty, value);
            }
        }

        public static void SelectedConfigurationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            StageEdit picker = sender as StageEdit;
            if (picker != null)
            {
                picker.OnSelectedConfigurationChanged();
            }
        }

        protected virtual void OnSelectedConfigurationChanged()
        {
        }

        public static DependencyProperty StageNameProperty = DependencyProperty.Register("StageName", typeof(string), typeof(StageEdit), new PropertyMetadata(new PropertyChangedCallback(StageNameChanged)));
        public string StageName
        {
            get
            {
                return (string)GetValue(StageNameProperty);
            }
            set
            {
                SetValue(StageNameProperty, value);
            }
        }

        public static void StageNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            StageEdit picker = sender as StageEdit;
            if (picker != null)
            {
                picker.OnStageNameChanged();
            }
        }

        protected virtual void OnStageNameChanged()
        {
        }

        public static DependencyProperty StageAssemblyIdProperty = DependencyProperty.Register("StageAssemblyId", typeof(long?), typeof(StageEdit), new PropertyMetadata(new PropertyChangedCallback(AssemblyIdChanged)));
        public long? StageAssemblyId
        {
            get
            {
                return (long?)GetValue(StageAssemblyIdProperty);
            }
            set
            {
                SetValue(StageAssemblyIdProperty, value);
            }
        }

        public static void AssemblyIdChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            StageEdit picker = sender as StageEdit;
            if (picker != null)
            {
                picker.OnStageAssemblyIdChanged();
            }
        }

        protected virtual void OnStageAssemblyIdChanged()
        {
        }


        public static DependencyProperty StageTypeProperty = DependencyProperty.Register("StageType", typeof(string), typeof(StageEdit), new PropertyMetadata(new PropertyChangedCallback(StageTypeChanged)));
        public string StageType
        {
            get
            {
                return (string)GetValue(StageTypeProperty);
            }
            set
            {
                SetValue(StageTypeProperty, value);
            }
        }

        public static void StageTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            StageEdit picker = sender as StageEdit;
            if (picker != null)
            {
                picker.OnStageTypeChanged();
            }
        }

        protected virtual void OnStageTypeChanged()
        {
            LoadParametersForStageType();
        }

        public static DependencyProperty StageParametersProperty = DependencyProperty.Register("StageParameters", typeof(ObservableCollection<StageParameter>), typeof(StageEdit), new PropertyMetadata(new PropertyChangedCallback(StageParametersChanged)));
        public ObservableCollection<StageParameter> StageParameters
        {
            get
            {
                return (ObservableCollection<StageParameter>)GetValue(StageParametersProperty);
            }
            set
            {
                SetValue(StageParametersProperty, value);
            }
        }

        public static void StageParametersChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            StageEdit picker = sender as StageEdit;
            if (picker != null)
            {
                picker.OnStageParametersChanged();
            }
        }

        protected virtual void OnStageParametersChanged()
        {
        }


        private void LoadParametersForStageType()
        {
            List<StageParameter> configParams = GetConfigurationParametersForType(PART_StageTypePicker.SelectedAssemblyId, PART_StageTypePicker.SelectedType);

            var parametersToDelete = StageParameters.Where(p => !configParams.Any(kvp => kvp.Name == p.Name));
            parametersToDelete.ToList().ForEach(p => StageParameters.Remove(p));

            BuildParameterGrid(configParams, PART_StageParametersGrid);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        void PART_StageTypePicker_SelectedTypeChanged(TypePicker<IProcessingStage> obj)
        {
            if (PART_StageTypePicker.SelectedType != null)
            {
                LoadParametersForStageType();
            }
        }

        private List<StageParameter> GetConfigurationParametersForType(long? assemblyId, string type)
        {
            List<StageParameter> result = new List<StageParameter>();

            if (assemblyId != null)
            {
                using (ConfigurationEntities entities = new ConfigurationEntities())
                {
                    byte[] assemblyBinary = entities.Assemblies
                                            .Where(av => av.Id == assemblyId)
                                            .Select(av => av.Executable)
                                            .SingleOrDefault();
                    if (assemblyBinary != null)
                    {
                        AppDomain tempDomain = AppDomain.CreateDomain("tempDomain");
                        AssemblyInspector inspector = tempDomain.CreateInstanceAndUnwrap(this.GetType().Assembly.FullName, typeof(AssemblyInspector).FullName) as AssemblyInspector;
                        result = inspector.Inspect(SelectedConfiguration, assemblyBinary, type);
                        AppDomain.Unload(tempDomain);
                    }
                }
            }
            return result;
        }

        private void BuildParameterGrid(List<StageParameter> configParams, Grid grid)
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            Enumerable.Range(0, configParams.Count).ToList().ForEach(n => grid.RowDefinitions.Add(new RowDefinition()));

            int rowNumber = 0;
            foreach (var parameter in configParams)
            {
                Control editControl;
                if (!StageParameters.Any(p => p.Name == parameter.Name))
                {
                    StageParameters.Add(new StageParameter() { Name = parameter.Name });
                }

                var stageParameter = StageParameters.Single(p => p.Name == parameter.Name);
                Binding editBinding = new Binding();
                editBinding.Source = stageParameter;
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
                grid.Children.Add(caption);
                grid.Children.Add(editControl);
                rowNumber++;
            }
        }
    }
}
