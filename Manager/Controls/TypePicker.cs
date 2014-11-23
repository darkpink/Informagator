using Acadian.Informagator.ProdProviders.Configuration;
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

namespace Acadian.Informagator.Manager.Controls
{
    public class TypePicker<T> : Control, INotifyPropertyChanged
    {

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

        public static DependencyProperty SelectedAssemblyNameProperty = DependencyProperty.Register("SelectedAssemblyName", typeof(string), typeof(TypePicker<T>), new PropertyMetadata(new PropertyChangedCallback(SelectedAssemblyNameChanged)));
        public string SelectedAssemblyName
        {
            get
            {
                return (string)GetValue(SelectedAssemblyNameProperty);
            }
            set
            {
                SetValue(SelectedAssemblyNameProperty, value);
            }
        }

        public static void SelectedAssemblyNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TypePicker<T> picker = sender as TypePicker<T>;
            if (picker != null)
            {
                picker.OnSelectedAssemblyNameChanged();
            }
        }

        private ObservableCollection<string> _assemblyNames;
        public ObservableCollection<string> AssemblyNames
        {
            get { return _assemblyNames; }
            private set { _assemblyNames = value; NotifyPropertyChanged("AssemblyNames"); }
        }

        private ObservableCollection<string> _assemblyVersions;
        public ObservableCollection<string> AssemblyVersions
        {
            get { return _assemblyVersions; }
            private set { _assemblyVersions = value; NotifyPropertyChanged("AssemblyVersions"); }
        }

        private ObservableCollection<string> _assemblyTypes;
        public ObservableCollection<string> AssemblyTypes
        {
            get { return _assemblyTypes; }
            private set { _assemblyTypes = value; NotifyPropertyChanged("AssemblyTypes"); }
        }

        protected void OnSelectedAssemblyNameChanged()
        {
            LoadVersionsForSelectedAssembly();
            EvaluateProperties();
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

        private bool _isVersionSelectAllowed;
        public bool IsVersionSelectAllowed
        {
            get { return _isVersionSelectAllowed; }
            private set
            {
                _isVersionSelectAllowed = value;
                NotifyPropertyChanged("IsVersionSelectAllowed");
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
            AssemblyNames = new ObservableCollection<string>();
            AssemblyVersions = new ObservableCollection<string>();
            AssemblyTypes = new ObservableCollection<string>();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                App.CurrentApplication.ActiveSystemConfigurationChanged += CurrentApplication_ActiveSystemConfigurationChanged;
                LoadAssembliesForSelectedConfiguraiton();
                EvaluateProperties();
            }
        }


        public static DependencyProperty SelectedAssemblyDotNetVersionProperty = DependencyProperty.Register("SelectedAssemblyDotNetVersion", typeof(string), typeof(TypePicker<T>), new PropertyMetadata(new PropertyChangedCallback(SelectedAssemblyDotNetVersionChanged)));
        public string SelectedAssemblyDotNetVersion
        {
            get
            {
                return (string)GetValue(SelectedAssemblyDotNetVersionProperty);
            }
            set
            {
                SetValue(SelectedAssemblyDotNetVersionProperty, value);
            }
        }

        public static void SelectedAssemblyDotNetVersionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TypePicker<T> picker = sender as TypePicker<T>;
            if (picker != null)
            {
                picker.OnSelectedAssemblyDotNetVersionChanged();
            }
        }

        protected virtual void OnSelectedAssemblyDotNetVersionChanged()
        {
            LoadTypesForSelectedAssemblyVersion();
            EvaluateProperties();
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
        }

            
        private void LoadAssembliesForSelectedConfiguraiton()
        {
            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                AssemblyNames.ToList().ForEach(n => AssemblyNames.Remove(n));
                entities.SystemConfigurations
                        .Include(c => c.AssemblySystemConfigurations)
                        .Where(c => c.Description == SelectedConfiguration)
                        .SelectMany( c=> c.AssemblySystemConfigurations)
                        .Select(asc => asc.AssemblyName)
                        .Distinct()
                        .OrderBy(n => n)
                        .ToList()
                        .ForEach(n => AssemblyNames.Add(n));
            }
        }

        private void LoadVersionsForSelectedAssembly()
        {
            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                AssemblyVersions.ToList().ForEach(v => AssemblyVersions.Remove(v));
                entities.SystemConfigurations
                        .Include(c => c.AssemblySystemConfigurations)
                        .Where(c => c.Description == SelectedConfiguration)
                        .SelectMany(c => c.AssemblySystemConfigurations)
                        .Where(asc => asc.AssemblyName == SelectedAssemblyName)
                        .Select(asc => asc.AssemblyDotNetVersion)
                        .Distinct()
                        .OrderBy(n => n)
                        .ToList()
                        .ForEach(v => AssemblyVersions.Add(v));
            }
        }
        private void LoadTypesForSelectedAssemblyVersion()
        {
            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                byte[] asmBin = entities.SystemConfigurations
                                            .Include(c => c.AssemblySystemConfigurations.Select(asc => asc.AssemblyVersion))
                                            .Where(c => c.Description == SelectedConfiguration)
                                            .SelectMany(c => c.AssemblySystemConfigurations)
                                            .Where(asc => asc.AssemblyName == SelectedAssemblyName && asc.AssemblyDotNetVersion == SelectedAssemblyDotNetVersion)
                                            .Select(asc => asc.AssemblyVersion.Executable)
                                            .SingleOrDefault();

                AssemblyTypes.ToList().ForEach(t => AssemblyTypes.Remove(t));
                if (asmBin != null)
                {
                    System.Reflection.Assembly asm = System.Reflection.Assembly.Load(asmBin);

                    asm.GetTypes()
                    .Where(t => t.GetInterfaces().Any(i => i.FullName == typeof(T).FullName)) //hack - the type picker only works for T == interface type
                    .Select(t => t.FullName)
                    .OrderBy(n => n)
                    .ToList()
                    .ForEach(t => AssemblyTypes.Add(t));
                }
            }
        }

        private void EvaluateProperties()
        {
            if (!AssemblyNames.Contains(SelectedAssemblyName)) { SelectedAssemblyName = null; }
            if (!AssemblyVersions.Contains(SelectedAssemblyDotNetVersion)) { SelectedAssemblyDotNetVersion = null; }
            if (!AssemblyTypes.Contains(SelectedType)) { SelectedType = null; }
            if (SelectedAssemblyName == null && IsVersionSelectAllowed) { IsVersionSelectAllowed = false; }
            if (SelectedAssemblyDotNetVersion == null && IsTypeSelectAllowed) { IsTypeSelectAllowed = false; }
            if (SelectedAssemblyName != null) { IsVersionSelectAllowed = true; }
            if (SelectedAssemblyDotNetVersion != null) { IsTypeSelectAllowed = true; }
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
    }
}
