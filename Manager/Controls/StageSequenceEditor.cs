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
    public class StageSequenceEditor : Control
    {
        static StageSequenceEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StageSequenceEditor), new FrameworkPropertyMetadata(typeof(StageSequenceEditor)));
            FocusableProperty.OverrideMetadata(typeof(StageSequenceEditor), new FrameworkPropertyMetadata(false));
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

                    StageEditor editor = new StageEditor();

                    Binding stageNameBinding = new Binding();
                    stageNameBinding.Path = new PropertyPath("EntityName");
                    stageNameBinding.Source = stg;
                    stageNameBinding.Mode = BindingMode.TwoWay;
                    BindingOperations.SetBinding(editor, StageEditor.EntityNameProperty, stageNameBinding);

                    Binding stageAssemblyNameBinding = new Binding();
                    stageAssemblyNameBinding.Path = new PropertyPath("AssemblyId");
                    stageAssemblyNameBinding.Source = stg;
                    stageAssemblyNameBinding.Mode = BindingMode.TwoWay;
                    BindingOperations.SetBinding(editor, StageEditor.AssemblyIdProperty, stageAssemblyNameBinding);

                    Binding stageTypeBinding = new Binding();
                    stageTypeBinding.Path = new PropertyPath("EntityType");
                    stageTypeBinding.Source = stg;
                    stageTypeBinding.Mode = BindingMode.TwoWay;
                    BindingOperations.SetBinding(editor, StageEditor.EntityTypeProperty, stageTypeBinding);

                    Binding stageParametersBinding = new Binding();
                    stageParametersBinding.Path = new PropertyPath("Parameters");
                    stageParametersBinding.Source = stg;
                    stageParametersBinding.Mode = BindingMode.TwoWay;
                    BindingOperations.SetBinding(editor, StageEditor.ParametersProperty, stageParametersBinding);

                    Grid.SetColumn(editor, 1);
                    Grid.SetRow(editor, index + 1);
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

        protected Button PART_SupplierButton { get; set; }
        protected Grid PART_PrimaryGrid { get; set; }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_SupplierButton = GetTemplateChild("PART_SupplierButton") as Button;
            PART_PrimaryGrid = GetTemplateChild("PART_PrimaryGrid") as Grid;

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
