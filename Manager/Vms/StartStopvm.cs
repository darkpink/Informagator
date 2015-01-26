using Informagator.Contracts;
using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Informagator.SystemStatus;
using Informagator.ProdProviders;

namespace Informagator.Manager.Vms
{
    public class StartStopVm : VmBase
    {
        public ObservableCollection<AutoRefreshingThreadStatus> Entities { get; protected set; }

        public StartStopVm()
        {
            Entities = new AutoRefreshingSystemStatus(new DatabaseConfigurationProvider());
        }
    }
}
