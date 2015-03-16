using Informagator.DBEntities.Configuration;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Informagator.Manager.Controls
{
    public class TypePicker<T> : Control, INotifyPropertyChanged
    {
        public class AssemblyIdCaption
        {
            public long AssemblyId { get; set; }
            public string Caption { get; set; }

            public override string ToString()
            {
                return Caption;
            }
        }

        public static readonly DependencyProperty SelectedConfigurationProperty = DependencyProperty.Register("SelectedConfiguration", typeof(string), typeof(TypePicker<T>), new FrameworkPropertyMetadata(new PropertyChangedCallback(SelectedConfigurationChanged)) { BindsTwoWayByDefault = true });
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
            var picker = sender as TypePicker<T>;
            if (picker != null)
            {
                picker.OnSelectedConfigurationChanged();
            }
        }

        private void OnSelectedConfigurationChanged()
        {
            LoadAssembliesForSelectedConfiguraiton();
        }

        public static DependencyProperty SelectedAssemblyIdProperty = DependencyProperty.Register("SelectedAssemblyId", typeof(long?), typeof(TypePicker<T>), new PropertyMetadata(new PropertyChangedCallback(SelectedAssemblyIdChanged)));
        
        public long? SelectedAssemblyId
        {
            get
            {
                return (long?)GetValue(SelectedAssemblyIdProperty);
            }
            set
            {
                SetValue(SelectedAssemblyIdProperty, value);
            }
        }
        
        private static void SelectedAssemblyIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TypePicker<T> picker = d as TypePicker<T>;
            picker.OnSelectedAssemblyIdChanged();
        }

        protected void OnSelectedAssemblyIdChanged()
        {
            EvaluateProperties();
            if (SelectedAssemblyId != null)
            {
                LoadTypesForSelectedAssemblyVersion();
            }

            if (PART_AssemblyComboBox != null)
            {
                PART_AssemblyComboBox.SelectedItem = SelectedAssemblyId == null ? null :
                                                     AssemblyIdsAndNames.SingleOrDefault(a => a.AssemblyId == SelectedAssemblyId);
            }
        }

        public static DependencyProperty SelectedTypeProperty = DependencyProperty.Register("SelectedType", typeof(string), typeof(TypePicker<T>), new PropertyMetadata(new PropertyChangedCallback(SelectedTypePropertyChanged)));
        public string SelectedType
        {
            get
            {
                return (string)GetValue(SelectedTypeProperty);
            }
            set
            {
                SetValue(SelectedTypeProperty, value);
            }
        }

        public static void SelectedTypePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TypePicker<T> picker = sender as TypePicker<T>;
            if (picker != null)
            {
                picker.OnSelectedTypeChanged();
            }
        }

        protected virtual void OnSelectedTypeChanged()
        {
            EvaluateProperties();
            if (SelectedTypeChanged != null)
            {
                SelectedTypeChanged(this);
            }

            if (PART_TypeComboBox != null)
            {
                PART_TypeComboBox.SelectedItem = SelectedType;
            }
        }

        private ObservableCollection<AssemblyIdCaption> _assemblyIdsAndNames;
        public ObservableCollection<AssemblyIdCaption> AssemblyIdsAndNames
        {
            get { return _assemblyIdsAndNames; }
            private set { _assemblyIdsAndNames = value; NotifyPropertyChanged("AssemblyIdsAndNames"); }
        }


        private ObservableCollection<string> _assemblyTypes;
        public ObservableCollection<string> AssemblyTypes
        {
            get { return _assemblyTypes; }
            private set { _assemblyTypes = value; NotifyPropertyChanged("AssemblyTypes"); }
        }

        private string _typeCaption;
        public string TypeCaption
        {
            get
            {
                return _typeCaption;
            }
            set
            {
                _typeCaption = value;
                NotifyPropertyChanged("TypeCaption");
            }
        }

        private bool _isTypeSelectAllowed;
        public bool IsTypeSelectAllowed
        {
            get { return _isTypeSelectAllowed; }
            private set
            {
                _isTypeSelectAllowed = value;
                NotifyPropertyChanged("IsTypeSelectAllowed");
            }
        }

        public TypePicker()
        {
            AssemblyIdsAndNames = new ObservableCollection<AssemblyIdCaption>();
            AssemblyTypes = new ObservableCollection<string>();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                App.CurrentApplication.ActiveSystemConfigurationChanged += CurrentApplication_ActiveSystemConfigurationChanged;
                LoadAssembliesForSelectedConfiguraiton();
                EvaluateProperties();
            }
        }


            
        private void LoadAssembliesForSelectedConfiguraiton()
        {
            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                AssemblyIdsAndNames.ToList().ForEach(n => AssemblyIdsAndNames.Remove(n));
                entities.SystemConfigurations
                        .Include(c => c.Assemblies)
                        .Where(c => c.Description == SelectedConfiguration)
                        .SelectMany(c => c.Assemblies)
                        .OrderBy(a => a.Name)
                        .Distinct()
                        .ToList()
                        .ForEach(n => AssemblyIdsAndNames.Add(new AssemblyIdCaption() { AssemblyId = n.Id, Caption = n.Name + ", " + n.Version }));

                //TODO - get fancy and try to get rid of non-unique name parts so it won't take up as much
                //width on the screen (Informagator.)
            }
        }

        private void LoadTypesForSelectedAssemblyVersion()
        {
            if (SelectedAssemblyId == null)
            {
                AssemblyTypes.Clear();
            }
            else
            {
                using (ConfigurationEntities entities = new ConfigurationEntities())
                {
                    var assemblyFromDB = entities.Assemblies.SingleOrDefault(a => a.Id == SelectedAssemblyId);

                    if (assemblyFromDB != null)
                    { 
                        //TODO - catch exceptions
                        byte[] assemblyBinary = assemblyFromDB.Executable;
                        Utilities.GetTypeNamesImplementingInterfaceFromAssembly(typeof(T), assemblyBinary)
                                 .OrderBy(n => n)
                                 .ToList()
                                 .ForEach(t => AssemblyTypes.Add(t));
                    }
                }
            }
        }

        private void EvaluateProperties()
        {
            if (!AssemblyIdsAndNames.Select(idn => (long?)idn.AssemblyId).Contains(SelectedAssemblyId)) { SelectedAssemblyId = null; }
            if (!AssemblyTypes.Contains(SelectedType)) { SelectedType = null; }
            if (SelectedAssemblyId != null) { IsTypeSelectAllowed = true; }
        }

        private void CurrentApplication_ActiveSystemConfigurationChanged()
        {
            LoadAssembliesForSelectedConfiguraiton();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event Action<TypePicker<T>> SelectedTypeChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected ComboBox PART_AssemblyComboBox { get; set; }

        protected ComboBox PART_TypeComboBox { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_AssemblyComboBox = GetTemplateChild("PART_AssemblyComboBox") as ComboBox;
            PART_TypeComboBox = GetTemplateChild("PART_TypeComboBox") as ComboBox;
            PART_AssemblyComboBox.SelectionChanged += AssemblyComboBox_SelectionChanged;
            PART_TypeComboBox.SelectionChanged += TypeComboBox_SelectionChanged;

            PART_AssemblyComboBox.SelectedItem = SelectedAssemblyId == null ? null :
                                                     AssemblyIdsAndNames.SingleOrDefault(a => a.AssemblyId == SelectedAssemblyId);
            PART_TypeComboBox.SelectedItem = SelectedType;
        }

        protected void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedType = e.AddedItems.OfType<string>().SingleOrDefault();
        }

        protected void AssemblyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedAssemblyId = e.AddedItems.OfType<AssemblyIdCaption>().Select(a => a.AssemblyId).SingleOrDefault();
        }
    }
}
