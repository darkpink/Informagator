using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Informagator.Manager.Controls
{
    public class TypeSelection : INotifyPropertyChanged
    {
        private string _selectedAssemblyName;
        private string _selectedAssemblyDotNetVersion;
        private string _selectedType;
        private string _selectedConfigurationName;
        private Type _typeFilter;
        private ObservableCollection<string> _availableAssemblyNames;
        private ObservableCollection<string> _availableAssemblyDotNetVersions;
        private ObservableCollection<string> _availableTypes;

        public string SelectedConfigurationName
        {
            get
            {
                return _selectedConfigurationName;
            }
            set
            {
                _selectedConfigurationName = value;
                LoadAssembliesForSelectedConfiguraiton();
                NotifyPropertyChanged("SelectedConfigurationName");
            }
        }

        public string SelectedAssemblyName
        {
            get
            {
                return _selectedAssemblyName;
            }
            set
            {
                if (value != _selectedAssemblyName)
                {
                    _selectedAssemblyName = value;
                    LoadVersionsForSelectedAssembly();
                    NotifyPropertyChanged("SelectedAssemblyName");
                }
            }
        }

        public string SelectedAssemblyDotNetVersion
        {
            get
            {
                return _selectedAssemblyDotNetVersion;
            }
            set
            {
                if (value != _selectedAssemblyDotNetVersion)
                {
                    _selectedAssemblyDotNetVersion = value;
                    LoadTypesForSelectedAssemblyVersion();
                    NotifyPropertyChanged("SelectedAssemblyDotNetVersion");
                }
            }
        }

        public string SelectedType
        {
            get
            {
                return _selectedType;
            }
            set
            {
                _selectedType = value;
                NotifyPropertyChanged("SelectedType");
            }
        }

        public ObservableCollection<string> AvailableAssemblyNames
        {
            get
            {
                return _availableAssemblyNames;
            }
            set
            {
                _availableAssemblyNames = value;
                NotifyPropertyChanged("AvailableAssemblyNames");
            }
        }

        public ObservableCollection<string> AvailableAssemblyDotNetVersions
        {
            get
            {
                return _availableAssemblyDotNetVersions;
            }
            set
            {
                _availableAssemblyDotNetVersions = value;
                NotifyPropertyChanged("AvailableAssemblyDotNetVersions");
            }
        }
        public ObservableCollection<string> AvailableTypes
        {
            get
            {
                return _availableTypes;
            }
            set
            {
                _availableTypes = value;
                NotifyPropertyChanged("AvailableTypes");
            }
        }

        public Type TypeFilter
        {
            get
            {
                return _typeFilter;
            }
            set
            {
                _typeFilter = value;
                LoadAssembliesForSelectedConfiguraiton();
                NotifyPropertyChanged("TypeFilter");
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

        public TypeSelection()
        {
            AvailableAssemblyNames = new ObservableCollection<string>();
            AvailableAssemblyDotNetVersions = new ObservableCollection<string>();
            AvailableTypes = new ObservableCollection<string>();
        }

        private void LoadAssembliesForSelectedConfiguraiton()
        {
            AvailableAssemblyNames.ToList().ForEach(n => AvailableAssemblyNames.Remove(n));

            if (SelectedConfigurationName != null && TypeFilter != null)
            {
                using (ConfigurationEntities entities = new ConfigurationEntities())
                {
                    entities.SystemConfigurations
                            .Include(c => c.Assemblies)
                            .Single(c => c.Description == SelectedConfigurationName)
                            .Assemblies
                            .Select(a => a.Name)
                            .Distinct()
                            .OrderBy(n => n)
                            .ToList()
                            .ForEach(n => AvailableAssemblyNames.Add(n));
                }
            }

            EvaluateProperties();
        }

        private void LoadVersionsForSelectedAssembly()
        {
            AvailableAssemblyDotNetVersions.ToList().ForEach(v => AvailableAssemblyDotNetVersions.Remove(v));

            if (SelectedAssemblyName != null && SelectedConfigurationName != null)
            {
                using (ConfigurationEntities entities = new ConfigurationEntities())
                {
                    entities.SystemConfigurations
                            .Include(c => c.Assemblies)
                            .Single(c => c.Description == SelectedConfigurationName)
                            .Assemblies
                            .Where(asc => asc.Name == SelectedAssemblyName)
                            .Select(asc => asc.Version)
                            .Distinct()
                            .OrderBy(n => n)
                            .ToList()
                            .ForEach(v => AvailableAssemblyDotNetVersions.Add(v));
                }
            }

            EvaluateProperties();
        }
        private void LoadTypesForSelectedAssemblyVersion()
        {
            AvailableTypes.ToList().ForEach(t => AvailableTypes.Remove(t));

            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                byte[] asmBin = entities.SystemConfigurations
                                            .Include(c => c.Assemblies)
                                            .Single(c => c.IsActive)
                                            .Assemblies
                                            .Where(asc => asc.Name == SelectedAssemblyName && asc.Version == SelectedAssemblyDotNetVersion)
                                            .Select(asc => asc.Executable)
                                            .SingleOrDefault();

                if (asmBin != null)
                {
                    System.Reflection.Assembly asm = System.Reflection.Assembly.Load(asmBin);

                    asm.GetTypes()
                    .Where(t => t.GetInterfaces().Any(i => i.FullName == TypeFilter.FullName)) //hack - the type picker only works for T == interface type
                    .Select(t => t.FullName)
                    .OrderBy(n => n)
                    .ToList()
                    .ForEach(t => AvailableTypes.Add(t));
                }
            }
            
            EvaluateProperties();
        }

        private void EvaluateProperties()
        {
            if (!AvailableAssemblyNames.Contains(SelectedAssemblyName)) { SelectedAssemblyName = null; }
            if (!AvailableAssemblyDotNetVersions.Contains(SelectedAssemblyDotNetVersion)) { SelectedAssemblyDotNetVersion = null; }
            if (!AvailableTypes.Contains(SelectedType)) { SelectedType = null; }
            if (SelectedAssemblyName == null && IsVersionSelectAllowed) { IsVersionSelectAllowed = false; }
            if (SelectedAssemblyDotNetVersion == null && IsTypeSelectAllowed) { IsTypeSelectAllowed = false; }
            if (SelectedAssemblyName != null) { IsVersionSelectAllowed = true; }
            if (SelectedAssemblyDotNetVersion != null) { IsTypeSelectAllowed = true; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
