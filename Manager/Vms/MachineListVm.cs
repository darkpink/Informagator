using Acadian.Informagator.ProdProviders.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Manager.Vms
{
    public class MachineListVm : ListPanelVmBase<Machine>
    {
        protected override Machine[] GetEntities()
        {
            Machine[] result;

            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                result = entities.Machines.Where(m => m.SystemConfiguration.Description == SelectedConfiguration).ToArray();
            }

            return result;
        }
    }
}
