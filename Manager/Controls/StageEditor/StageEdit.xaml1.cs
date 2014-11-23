using System;
using System.Collections.Generic;
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

namespace Acadian.Informagator.Manager.Controls
{
    /// <summary>
    /// Interaction logic for StageEdit.xaml
    /// </summary>
    public partial class StageEdit : UserControl
    {
        public StageEdit()
        {
            InitializeComponent();
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


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_IsExpandedClickBorder.MouseDown += delegate(object sender, MouseButtonEventArgs args) { IsExpanded = !IsExpanded; };
        }

    }
}
