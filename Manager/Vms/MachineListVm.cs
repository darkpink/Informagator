using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Manager.Vms
{
    public class MachineListVm : ListPanelVmBase<Informagator.DBEntities.Configuration.Machine>
    {
        protected override Informagator.DBEntities.Configuration.Machine[] GetEntities()
        {
            Informagator.DBEntities.Configuration.Machine[] result;

            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                result = entities.Machines.Where(m => m.SystemConfiguration.Description == ConfigurationSelection.SelectedConfiguration).ToArray();
            }

            return result;
        }
    }
}
