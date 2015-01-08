using Informagator.ProdProviders.Configuration;
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
            ErrorHandlerParameters = new ObservableCollection<StageParameter>();
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

        public static DependencyProperty StageAssemblyNameProperty = DependencyProperty.Register("StageAssemblyName", typeof(string), typeof(StageEdit), new PropertyMetadata(new PropertyChangedCallback(StageAssemblyNameChanged)));
        public string StageAssemblyName
        {
            get
            {
                return (string)GetValue(StageAssemblyNameProperty);
            }
            set
            {
                SetValue(StageAssemblyNameProperty, value);
            }
        }

        public static void StageAssemblyNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            StageEdit picker = sender as StageEdit;
            if (picker != null)
            {
                picker.OnStageAssemblyNameChanged();
            }
        }

        protected virtual void OnStageAssemblyNameChanged()
        {
        }

        public static DependencyProperty StageAssemblyDotNetVersionProperty = DependencyProperty.Register("StageAssemblyDotNetVersion", typeof(string), typeof(StageEdit), new PropertyMetadata(new PropertyChangedCallback(StageAssemblyDotNetVersionChanged)));
        public string StageAssemblyDotNetVersion
        {
            get
            {
                return (string)GetValue(StageAssemblyDotNetVersionProperty);
            }
            set
            {
                SetValue(StageAssemblyDotNetVersionProperty, value);
            }
        }

        public static void StageAssemblyDotNetVersionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            StageEdit picker = sender as StageEdit;
            if (picker != null)
            {
                picker.OnStageAssemblyDotNetVersionChanged();
            }
        }

        protected virtual void OnStageAssemblyDotNetVersionChanged()
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

        public static DependencyProperty ErrorHandlerParametersProperty = DependencyProperty.Register("ErrorHandlerParameters", typeof(ObservableCollection<StageParameter>), typeof(StageEdit), new PropertyMetadata(new PropertyChangedCallback(ErrorHandlerParametersChanged)));
        public ObservableCollection<StageParameter> ErrorHandlerParameters
        {
            get
            {
                return (ObservableCollection<StageParameter>)GetValue(ErrorHandlerParametersProperty);
            }
            set
            {
                SetValue(ErrorHandlerParametersProperty, value);
            }
        }

        public static void ErrorHandlerParametersChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            StageEdit picker = sender as StageEdit;
            if (picker != null)
            {
                picker.OnErrorHandlerParametersChanged();
            }
        }

        protected virtual void OnErrorHandlerParametersChanged()
        {
        }


        private void LoadParametersForStageType()
        {
            Dictionary<string, Type> configParams = GetConfigurationParametersForType(PART_StageTypePicker.SelectedAssemblyName, PART_StageTypePicker.SelectedAssemblyDotNetVersion,
                                              PART_StageTypePicker.SelectedType);

            var parametersToDelete = StageParameters.Where(p => !configParams.Any(kvp => kvp.Key == p.Name));
            parametersToDelete.ToList().ForEach(p => StageParameters.Remove(p));

            BuildParameterGrid(configParams, PART_StageParametersGrid);
        }

        public static DependencyProperty ErrorHandlerNameProperty = DependencyProperty.Register("ErrorHandlerName", typeof(string), typeof(StageEdit), new PropertyMetadata(new PropertyChangedCallback(ErrorHandlerNameChanged)));
        public string ErrorHandlerName
        {
            get
            {
                return (string)GetValue(ErrorHandlerNameProperty);
            }
            set
            {
                SetValue(ErrorHandlerNameProperty, value);
            }
        }

        public static void ErrorHandlerNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            StageEdit picker = sender as StageEdit;
            if (picker != null)
            {
                picker.OnErrorHandlerNameChanged();
            }
        }

        protected virtual void OnErrorHandlerNameChanged()
        {
        }

        public static DependencyProperty ErrorHandlerAssemblyNameProperty = DependencyProperty.Register("ErrorHandlerAssemblyName", typeof(string), typeof(StageEdit), new PropertyMetadata(new PropertyChangedCallback(ErrorHandlerAssemblyNameChanged)));
        public string ErrorHandlerAssemblyName
        {
            get
            {
                return (string)GetValue(ErrorHandlerAssemblyNameProperty);
            }
            set
            {
                SetValue(ErrorHandlerAssemblyNameProperty, value);
            }
        }

        public static void ErrorHandlerAssemblyNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            StageEdit picker = sender as StageEdit;
            if (picker != null)
            {
                picker.OnErrorHandlerAssemblyNameChanged();
            }
        }

        protected virtual void OnErrorHandlerAssemblyNameChanged()
        {
        }

        public static DependencyProperty ErrorHandlerAssemblyDotNetVersionProperty = DependencyProperty.Register("ErrorHandlerAssemblyDotNetVersion", typeof(string), typeof(StageEdit), new PropertyMetadata(new PropertyChangedCallback(ErrorHandlerAssemblyDotNetVersionChanged)));
        public string ErrorHandlerAssemblyDotNetVersion
        {
            get
            {
                return (string)GetValue(ErrorHandlerAssemblyDotNetVersionProperty);
            }
            set
            {
                SetValue(ErrorHandlerAssemblyDotNetVersionProperty, value);
            }
        }

        public static void ErrorHandlerAssemblyDotNetVersionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            StageEdit picker = sender as StageEdit;
            if (picker != null)
            {
                picker.OnErrorHandlerAssemblyDotNetVersionChanged();
            }
        }

        protected virtual void OnErrorHandlerAssemblyDotNetVersionChanged()
        {
        }

        public static DependencyProperty ErrorHandlerTypeProperty = DependencyProperty.Register("ErrorHandlerType", typeof(string), typeof(StageEdit), new PropertyMetadata(new PropertyChangedCallback(ErrorHandlerTypeChanged)));
        public string ErrorHandlerType
        {
            get
            {
                return (string)GetValue(ErrorHandlerTypeProperty);
            }
            set
            {
                SetValue(ErrorHandlerTypeProperty, value);
            }
        }

        public static void ErrorHandlerTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            StageEdit picker = sender as StageEdit;
            if (picker != null)
            {
                picker.OnErrorHandlerTypeChanged();
            }
        }

        protected virtual void OnErrorHandlerTypeChanged()
        {
            LoadParametersForErrorHandlerType();
        }

        private void LoadParametersForErrorHandlerType()
        {
            Dictionary<string, Type> configParams = GetConfigurationParametersForType(ErrorHandlerAssemblyName, ErrorHandlerAssemblyDotNetVersion, ErrorHandlerType);

            var parametersToDelete = ErrorHandlerParameters.Where(p => !configParams.Any(kvp => kvp.Key == p.Name));
            parametersToDelete.ToList().ForEach(p => ErrorHandlerParameters.Remove(p));

            BuildParameterGrid(configParams, PART_ErrorHandlerParametersGrid);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_IsExpandedClickBorder.MouseDown += delegate(object sender, MouseButtonEventArgs args) { IsExpanded = !IsExpanded; };
            PART_StageTypePicker.SelectedTypeChanged += PART_StageTypePicker_SelectedTypeChanged;
        }

        void PART_StageTypePicker_SelectedTypeChanged(TypePicker<Contracts.IProcessingStage> obj)
        {
            if (PART_StageTypePicker.SelectedType != null)
            {
                LoadParametersForStageType();
            }
        }

        private Dictionary<string, Type> GetConfigurationParametersForType(string name, string version, string type)
        {
            Dictionary<string, Type> result = new Dictionary<string, Type>();

            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                byte[] assemblyBinary = entities.AssemblyVersions
                                        .Where(av => av.AssemblyName == name &&
                                                               av.AssemblyDotNetVersion == version &&
                                                               av.AssemblySystemConfigurations.Any(asc => asc.SystemConfiguration.Description == SelectedConfiguration)
                                               )
                                         .Select(av => av.Executable)
                                         .SingleOrDefault();
                AppDomain tempDomain = AppDomain.CreateDomain("tempDomain");
                AssemblyInspector inspector = tempDomain.CreateInstanceAndUnwrap(this.GetType().Assembly.FullName, typeof(AssemblyInspector).FullName) as AssemblyInspector;
                result = inspector.Inspect(SelectedConfiguration, assemblyBinary, type);
                AppDomain.Unload(tempDomain);
            }

            return result;
        }

        private void BuildParameterGrid(Dictionary<string, Type> configParams, Grid grid)
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            Enumerable.Range(0, configParams.Count).ToList().ForEach(n => grid.RowDefinitions.Add(new RowDefinition()));

            int rowNumber = 0;
            foreach (var name in configParams.Keys)
            {
                Control editControl;
                if (!StageParameters.Any(p => p.Name == name))
                {
                    StageParameters.Add(new StageParameter() { Name = name });
                }

                var stageParameter = StageParameters.Single(p => p.Name == name);
                Binding editBinding = new Binding();
                editBinding.Source = stageParameter;
                editBinding.Path = new PropertyPath("Value");
                editBinding.Mode = BindingMode.TwoWay;
                editBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

                if (configParams[name] == typeof(bool))
                {
                    editControl = new CheckBox();
                    BindingOperations.SetBinding(editControl, CheckBox.IsCheckedProperty, editBinding);
                }
                else
                {
                    editControl = new TextBox();
                    BindingOperations.SetBinding(editControl, TextBox.TextProperty, editBinding);
                }

                TextBlock caption = new TextBlock() { HorizontalAlignment = System.Windows.HorizontalAlignment.Right, Text = name };

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
