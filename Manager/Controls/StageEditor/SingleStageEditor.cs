using Acadian.Informagator.Configuration;
using Acadian.Informagator.ProdProviders.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Acadian.Informagator.Manager.Controls.StageEditor
{
    public class SingleStageEditor : Control
    {
        static SingleStageEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SingleStageEditor), new FrameworkPropertyMetadata(typeof(SingleStageEditor)));
            //FocusableProperty.OverrideMetadata(typeof(SingleStageEditor), new FrameworkPropertyMetadata(false));
        }

        public static DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(SingleStageEditor), new PropertyMetadata(new PropertyChangedCallback(IsExpandedChanged)));
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
            SingleStageEditor editor = sender as SingleStageEditor;
            if (editor != null)
            {
                editor.IsExpandedChanged();
            }
        }
        protected virtual void IsExpandedChanged()
        {
        }

        public string SelectedSystemConfiguration { get; set; }

        public static DependencyProperty StageProperty = DependencyProperty.Register("Stage", typeof(Stage), typeof(SingleStageEditor), new PropertyMetadata(new PropertyChangedCallback(StageChanged)));
        public Stage Stage
        {
            get
            {
                return (Stage)GetValue(StageProperty);
            }
            set
            {
                SetValue(StageProperty, value);
            }
        }
        public static void StageChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SingleStageEditor editor = sender as SingleStageEditor;
            if (editor != null)
            {
                editor.StageChanged();
            }
        }
        protected virtual void StageChanged()
        {
        }

        protected Grid PART_StageParametersGrid { get; set; }
        protected Grid PART_ErrorHandlerParametersGrid { get; set; }
        protected StageTypePicker PART_StageTypePicker { get; set; }
        protected ErrorHandlerPicker PART_ErrorHandlerTypePicker { get; set; }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_ErrorHandlerParametersGrid = GetTemplateChild("PART_ErrorHandlerParametersGrid") as Grid;
            PART_StageParametersGrid = GetTemplateChild("PART_StageParametersGrid") as Grid;
            PART_ErrorHandlerTypePicker = GetTemplateChild("PART_ErrorHandlerTypePicker") as ErrorHandlerPicker;
            PART_StageTypePicker = GetTemplateChild("PART_StageTypePicker") as StageTypePicker;

            PART_ErrorHandlerTypePicker.SelectedTypeChanged += PART_ErrorHandlerTypePicker_SelectedTypeChanged;
            PART_StageTypePicker.SelectedTypeChanged += PART_StageTypePicker_SelectedTypeChanged;
        }

        protected void PART_StageTypePicker_SelectedTypeChanged(TypePicker<Contracts.IProcessingStage> obj)
        {
            Dictionary<string, Type> configParams = GetConfigurationParametersForType(PART_StageTypePicker.SelectedAssemblyName, PART_StageTypePicker.SelectedAssemblyDotNetVersion,
                                              PART_StageTypePicker.SelectedType);
            BuildParameterGrid(configParams, PART_StageParametersGrid);
        }


        protected void PART_ErrorHandlerTypePicker_SelectedTypeChanged(TypePicker<Contracts.IMessageErrorHandler> obj)
        {
            
        }

        private Dictionary<string, Type> GetConfigurationParametersForType(string name, string version, string type)
        {
            Dictionary<string, Type> result = new Dictionary<string, Type>();

            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                byte[] assemblyBinary = entities.AssemblyVersions
                                        .Where(av => av.AssemblyName == name &&
                                                               av.AssemblyDotNetVersion == version &&
                                                               av.AssemblySystemConfigurations.Any(asc => asc.SystemConfiguration.Description == SelectedSystemConfiguration)
                                               )
                                         .Select(av => av.Executable)
                                         .SingleOrDefault();
                AppDomain tempDomain = AppDomain.CreateDomain("tempDomain");
                AssemblyInspector inspector = tempDomain.CreateInstanceAndUnwrap(this.GetType().Assembly.FullName, typeof(AssemblyInspector).FullName) as AssemblyInspector;
                result = inspector.Inspect(SelectedSystemConfiguration, assemblyBinary, type);
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
            foreach(var name in configParams.Keys)
            {
                Control editControl;
                TextBlock caption = new TextBlock() { HorizontalAlignment = System.Windows.HorizontalAlignment.Right, Text = name };
                editControl = new TextBox();
                Grid.SetColumn(editControl, 1);
                Grid.SetRow(caption, rowNumber);
                Grid.SetRow(editControl, rowNumber);
                grid.Children.Add(caption);
                grid.Children.Add(editControl);
                rowNumber++;
            }
        }

        public class AssemblyInspector : MarshalByRefObject
        {
            protected string SelectedSystemConfiguration { get; set; }
            
            public Dictionary<string, Type> Inspect(string selectedSystemConfiguration, byte[] toReflect, string type)
            {
                Dictionary<string, Type> result = new Dictionary<string, Type>();

                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
                System.Reflection.Assembly asm = System.Reflection.Assembly.Load(toReflect);
                
                Type t = asm.GetType(type);
                var x = t.GetProperties().SelectMany(p => p.CustomAttributes);
                var propsWithAttribute = t.GetProperties().Where(p => p.CustomAttributes.Any(a => a.AttributeType.FullName == typeof(ConfigurationParameterAttribute).FullName));
                foreach (PropertyInfo info in propsWithAttribute)
                {
                    ConfigurationParameterAttribute attr = (ConfigurationParameterAttribute)info.GetCustomAttributes().Single(a => a.GetType() == typeof(ConfigurationParameterAttribute));
                    string displayName = attr.DisplayName ?? info.Name;
                    Type propType = info.PropertyType;
                    result.Add(displayName, propType);
                }
                
                return result;
            }

            protected System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
            {
                System.Reflection.Assembly result = null;

                var asmName = new System.Reflection.AssemblyName(args.Name);
                string n = asmName.Name + ".dll";
                string v = asmName.Version.ToString();

                using (ConfigurationEntities entities = new ConfigurationEntities())
                {
                    byte[] assemblyBinary = entities.AssemblyVersions
                                            .Where(av => av.AssemblyName == n &&
                                                                   av.AssemblyDotNetVersion == v &&
                                                                   av.AssemblySystemConfigurations.Any(asc => asc.SystemConfiguration.Description == SelectedSystemConfiguration)
                                                   )
                                             .Select(av => av.Executable)
                                             .SingleOrDefault();
                    result = System.Reflection.Assembly.ReflectionOnlyLoad(assemblyBinary);
                }

                return result;
            }
        }
    }
}
