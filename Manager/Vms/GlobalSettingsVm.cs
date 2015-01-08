using Informagator.ProdProviders.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Manager.Vms
{
    public class GlobalSettingsVm : VmBase
    {
        public ObservableCollection<GlobalSetting> GlobalSettings { get; protected set; }

        public GlobalSettingsVm()
        {
            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                GlobalSettings = new ObservableCollection<GlobalSetting>(entities.GlobalSettings);
            }
        }

    }
}
