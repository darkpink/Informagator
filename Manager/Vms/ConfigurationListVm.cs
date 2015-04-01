using Informagator.Manager.Commands;
using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;

namespace Informagator.Manager.Vms
{
    public class ConfigurationListVm : VmBase
    {
        public ObservableCollection<SystemConfiguration> SystemConfigurations { get; protected set; }

        public override void Refresh()
        {
            base.Refresh();

            RefreshSystemConfigurations();
        }
        public ICommand DeleteConfiguration
        {
            get
            {
                return new DeleteEntityCommand<SystemConfiguration>(entities => entities.SystemConfigurations, (SystemConfigurations, id) => SystemConfigurations.Single(w => w.Id == (long)id), null, Refresh);
            }
        }

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
    }
}
