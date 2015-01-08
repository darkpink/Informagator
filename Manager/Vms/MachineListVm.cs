using Informagator.ProdProviders.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Manager.Vms
{
    public class MachineListVm : ListPanelVmBase<Informagator.ProdProviders.Configuration.Machine>
    {
        protected override Informagator.ProdProviders.Configuration.Machine[] GetEntities()
        {
            Informagator.ProdProviders.Configuration.Machine[] result;

            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                result = entities.Machines.Where(m => m.SystemConfiguration.Description == SelectedConfiguration).ToArray();
            }

            return result;
        }
    }
}
