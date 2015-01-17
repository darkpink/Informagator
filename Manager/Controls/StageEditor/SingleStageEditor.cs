using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Informagator.Manager.Controls.StageEditor
{
    public class SingleStageEditor : Control
    {
        static SingleStageEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SingleStageEditor), new FrameworkPropertyMetadata(typeof(SingleStageEditor)));
            FocusableProperty.OverrideMetadata(typeof(SingleStageEditor), new FrameworkPropertyMetadata(false));
        }

        //public static DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(SingleStageEditor), new PropertyMetadata(new PropertyChangedCallback(IsExpandedChanged)));
        //public bool IsExpanded
        //{
        //    get
        //    {
        //        return (bool)GetValue(IsExpandedProperty);
        //    }
        //    set
        //    {
        //        SetValue(IsExpandedProperty, value);
        //    }
        //}
        //public static void IsExpandedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    SingleStageEditor editor = sender as SingleStageEditor;
        //    if (editor != null)
        //    {
        //        editor.IsExpandedChanged();
        //    }
        //}
        //protected virtual void IsExpandedChanged()
        //{
        //}

        //protected Border PART_IsExpandedClickBorder { get; set; }
        //protected Grid PART_StageParametersGrid { get; set; }
        //protected Grid PART_ErrorHandlerParametersGrid { get; set; }
        //protected StageTypePicker PART_StageTypePicker { get; set; }
        //protected ErrorHandlerPicker PART_ErrorHandlerTypePicker { get; set; }
        //public override void OnApplyTemplate()
        //{
        //    base.OnApplyTemplate();

        //    PART_IsExpandedClickBorder = GetTemplateChild("PART_IsExpandedClickBorder") as Border;
        //    PART_ErrorHandlerParametersGrid = GetTemplateChild("PART_ErrorHandlerParametersGrid") as Grid;
        //    PART_StageParametersGrid = GetTemplateChild("PART_StageParametersGrid") as Grid;
        //    PART_ErrorHandlerTypePicker = GetTemplateChild("PART_ErrorHandlerTypePicker") as ErrorHandlerPicker;
        //    PART_StageTypePicker = GetTemplateChild("PART_StageTypePicker") as StageTypePicker;

        //    PART_ErrorHandlerTypePicker.SelectedTypeChanged += PART_ErrorHandlerTypePicker_SelectedTypeChanged;
        //    PART_StageTypePicker.SelectedTypeChanged += PART_StageTypePicker_SelectedTypeChanged;

        //    PART_IsExpandedClickBorder.MouseDown += delegate(object sender, MouseButtonEventArgs args) { IsExpanded = !IsExpanded; };
        //}

        //protected void PART_StageTypePicker_SelectedTypeChanged(TypePicker obj)
        //{
        //    Dictionary<string, Type> configParams = GetConfigurationParametersForType(PART_StageTypePicker.SelectedAssemblyName, PART_StageTypePicker.SelectedAssemblyDotNetVersion,
        //                                      PART_StageTypePicker.SelectedType);

        //    var parametersToDelete = Stage.StageParameters.Where(p => !configParams.Any(kvp => kvp.Key == p.Name));
        //    parametersToDelete.ToList().ForEach(p => Stage.StageParameters.Remove(p));

        //    BuildParameterGrid(configParams, PART_StageParametersGrid);
        //}


        //protected void PART_ErrorHandlerTypePicker_SelectedTypeChanged(TypePicker obj)
        //{
        //    Dictionary<string, Type> configParams = GetConfigurationParametersForType(PART_ErrorHandlerTypePicker.SelectedAssemblyName, PART_ErrorHandlerTypePicker.SelectedAssemblyDotNetVersion,
        //                                      PART_ErrorHandlerTypePicker.SelectedType);

        //    var parametersToDelete = Stage.ErrorHandlerParameters.Where(p => !configParams.Any(kvp => kvp.Key == p.Name));
        //    parametersToDelete.ToList().ForEach(p => Stage.ErrorHandlerParameters.Remove(p));

        //    BuildParameterGrid(configParams, PART_ErrorHandlerParametersGrid);

        //}

        //private Dictionary<string, Type> GetConfigurationParametersForType(string name, string version, string type)
        //{
        //    Dictionary<string, Type> result = new Dictionary<string, Type>();

        //    using (ConfigurationEntities entities = new ConfigurationEntities())
        //    {
        //        byte[] assemblyBinary = entities.AssemblyVersions
        //                                .Where(av => av.AssemblyName == name &&
        //                                                       av.AssemblyDotNetVersion == version &&
        //                                                       av.AssemblySystemConfigurations.Any(asc => asc.SystemConfiguration.Description == SelectedSystemConfiguration)
        //                                       )
        //                                 .Select(av => av.Executable)
        //                                 .SingleOrDefault();
        //        AppDomain tempDomain = AppDomain.CreateDomain("tempDomain");
        //        AssemblyInspector inspector = tempDomain.CreateInstanceAndUnwrap(this.GetType().Assembly.FullName, typeof(AssemblyInspector).FullName) as AssemblyInspector;
        //        result = inspector.Inspect(SelectedSystemConfiguration, assemblyBinary, type);
        //        AppDomain.Unload(tempDomain);
        //    }

        //    return result;
        //}

        //private void BuildParameterGrid(Dictionary<string, Type> configParams, Grid grid)
        //{
        //    grid.Children.Clear();
        //    grid.RowDefinitions.Clear();
        //    Enumerable.Range(0, configParams.Count).ToList().ForEach(n => grid.RowDefinitions.Add(new RowDefinition()));

        //    int rowNumber = 0;
        //    foreach (var name in configParams.Keys)
        //    {
        //        Control editControl;
        //        if (!Stage.StageParameters.Any(p => p.Name == name))
        //        {
        //            Stage.StageParameters.Add(new StageParameter() { Name = name });
        //        }

        //        var stageParameter = Stage.StageParameters.Single(p => p.Name == name);
        //        Binding editBinding = new Binding();
        //        editBinding.Source = stageParameter;
        //        editBinding.Path = new PropertyPath("Value");
        //        editBinding.Mode = BindingMode.TwoWay;
        //        editBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

        //        if (configParams[name] == typeof(bool))
        //        {
        //            editControl = new CheckBox();
        //            BindingOperations.SetBinding(editControl, CheckBox.IsCheckedProperty, editBinding);
        //        }
        //        else
        //        {
        //            editControl = new TextBox();
        //            BindingOperations.SetBinding(editControl, TextBox.TextProperty, editBinding);
        //        }

        //        TextBlock caption = new TextBlock() { HorizontalAlignment = System.Windows.HorizontalAlignment.Right, Text = name };

        //        Grid.SetColumn(editControl, 1);
        //        Grid.SetRow(caption, rowNumber);
        //        Grid.SetRow(editControl, rowNumber);
        //        grid.Children.Add(caption);
        //        grid.Children.Add(editControl);
        //        rowNumber++;
        //    }
        //}
    }
}
