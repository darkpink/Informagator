using Informagator.Manager.Commands;
using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Informagator.Manager.Vms
{
    public class ConfigurationListVm : SelectedConfigurationVmBase
    {
        public ObservableCollection<SystemConfiguration> SystemConfigurations { get; protected set; }

        public ICommand ChangeActiveSystemConfiguration
        {
            get
            {
                return new ChangeActiveSystemConfigurationCommand(); ;
            }
        }

        protected void RefreshSystemConfigurations()
        {
            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                SystemConfigurations = new ObservableCollection<SystemConfiguration>(entities.SystemConfigurations);
                NotifyPropertyChanged("SystemConfigurations");
            }
        }
        
        public ConfigurationListVm()
            : base()
        {
            RefreshSystemConfigurations();
        }

        protected override void ActiveSystemConfigurationChanged()
        {
            base.ActiveSystemConfigurationChanged();
            RefreshSystemConfigurations();
        }
    }
}
