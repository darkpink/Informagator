using Acadian.Informagator.ProdProviders.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Acadian.Informagator.Manager.Vms
{
    public class WorkerListVm : ListPanelVmBase<Worker>
    {
        protected override Worker[] GetEntities()
        {
            Worker[] result;

            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                result = entities.Workers.Include(t => t.Machine).Where(w => w.Machine.SystemConfiguration.Description == SelectedConfiguration).ToArray();
            }

            return result;
        }
    }
}
