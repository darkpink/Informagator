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
    /// Interaction logic for StageSequenceEditor.xaml
    /// </summary>
    public partial class StageSequenceEditor : UserControl
    {
        public StageSequenceEditor()
        {
            InitializeComponent();
            Stages = new ObservableCollection<Stage>();
        }

        public static DependencyProperty SelectedSystemConfigurationProperty = DependencyProperty.Register("SelectedSystemConfiguration", typeof(string), typeof(StageSequenceEditor), new PropertyMetadata(new PropertyChangedCallback(SelectedSystemConfigurationChanged)));
        public string SelectedSystemConfiguration
        {
            get
            {
                return (string)GetValue(SelectedSystemConfigurationProperty);
            }
            set
            {
                SetValue(SelectedSystemConfigurationProperty, value);
            }
        }

        public static void SelectedSystemConfigurationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
        }
        protected virtual void SelectedSystemConfigurationChanged()
        {
        }

        public static DependencyProperty StagesProperty = DependencyProperty.Register("Stages", typeof(ObservableCollection<Stage>), typeof(StageSequenceEditor), new PropertyMetadata(new PropertyChangedCallback(StagesChanged)));
        public ObservableCollection<Stage> Stages
        {
            get
            {
                return (ObservableCollection<Stage>)GetValue(StagesProperty);
            }
            set
            {
                SetValue(StagesProperty, value);
            }
        }
        public static void StagesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            StageSequenceEditor editor = sender as StageSequenceEditor;
            if (editor != null)
            {
                editor.StagesChanged();
            }
        }
        protected virtual void StagesChanged()
        {
            if (Stages != null)
            {
                Stages.CollectionChanged += Stages_CollectionChanged;
                BuildStages();
            }
        }

        private void BuildStages()
        {
            if (PART_PrimaryGrid != null)
            {
                PART_PrimaryGrid.Children.Clear();
                PART_PrimaryGrid.Children.Add(PART_SupplierButton);

                PART_PrimaryGrid.RowDefinitions.Clear();
                int newNumberOfRowDefinitions = Stages == null ? 1 : Stages.Count + 1;
                Enumerable.Range(0, newNumberOfRowDefinitions).ToList().ForEach(
                    n => PART_PrimaryGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto }));

                for (int index = 0; index < (Stages == null ? 0 : Stages.Count); index++)
                {
                    Stage stg = Stages[index];

                    StageEdit editor = new StageEdit();

                    Binding selectedConfigurationBinding = new Binding();
                    selectedConfigurationBinding.Path = new PropertyPath("SelectedSystemConfiguration");
                    selectedConfigurationBinding.Source = this;
                    selectedConfigurationBinding.Mode = BindingMode.TwoWay;
                    BindingOperations.SetBinding(editor, StageEdit.SelectedConfigurationProperty, selectedConfigurationBinding);

                    Binding stageNameBinding = new Binding();
                    stageNameBinding.Path = new PropertyPath("Name");
                    stageNameBinding.Source = stg;
                    stageNameBinding.Mode = BindingMode.TwoWay;
                    BindingOperations.SetBinding(editor, StageEdit.StageNameProperty, stageNameBinding);

                    Binding stageAssemblyNameBinding = new Binding();
                    stageAssemblyNameBinding.Path = new PropertyPath("StageAssemblyId");
                    stageAssemblyNameBinding.Source = stg;
                    stageAssemblyNameBinding.Mode = BindingMode.TwoWay;
                    BindingOperations.SetBinding(editor, StageEdit.StageAssemblyIdProperty, stageAssemblyNameBinding);

                    Binding stageTypeBinding = new Binding();
                    stageTypeBinding.Path = new PropertyPath("StageType");
                    stageTypeBinding.Source = stg;
                    stageTypeBinding.Mode = BindingMode.TwoWay;
                    BindingOperations.SetBinding(editor, StageEdit.StageTypeProperty, stageTypeBinding);

                    Binding stageParametersBinding = new Binding();
                    stageParametersBinding.Path = new PropertyPath("StageParameters");
                    stageParametersBinding.Source = stg;
                    stageParametersBinding.Mode = BindingMode.TwoWay;
                    BindingOperations.SetBinding(editor, StageEdit.StageParametersProperty, stageParametersBinding);

                    Grid.SetColumn(editor, 1);
                    Grid.SetRow(editor, index + 1);
                    editor.SelectedConfiguration = SelectedSystemConfiguration;
                    PART_PrimaryGrid.Children.Add(editor);

                    Button addButton = new Button();
                    addButton.Style = PART_PrimaryGrid.Resources["AddStage"] as Style;
                    addButton.DataContext = stg;
                    addButton.Click += AddButton_Click;
                    Grid.SetRow(addButton, index + 1);
                    PART_PrimaryGrid.Children.Add(addButton);

                    Button removeButton = new Button();
                    removeButton.Style = PART_PrimaryGrid.Resources["RemoveStage"] as Style;
                    removeButton.DataContext = stg;
                    removeButton.Click += RemoveButton_Click;
                    Grid.SetRow(removeButton, index + 1);
                    Grid.SetColumn(removeButton, 2);
                    PART_PrimaryGrid.Children.Add(removeButton);
                }
            }
        }

        protected void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Stage stg = (sender as Button).DataContext as Stage;
            Stages.Remove(stg);
        }

        protected void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Stage stg = (sender as Button).DataContext as Stage;
            int index = Stages.IndexOf(stg);
            Stage newStage = new Stage();
            Stages.Insert(index + 1, newStage);
        }

        public void Stages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (PART_PrimaryGrid != null)
            {
                BuildStages();
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_SupplierButton.Click += PART_SupplierButton_Click;

            BuildStages();
        }

        protected void PART_SupplierButton_Click(object sender, RoutedEventArgs e)
        {
            Stage newStage = new Stage();
            Stages.Insert(0, newStage);
        }
    }
}
