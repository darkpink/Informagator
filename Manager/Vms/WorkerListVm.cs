using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Windows.Input;
using Informagator.Manager.Commands;
using Informagator.DBEntities.Configuration;

namespace Informagator.Manager.Vms
{
    public class WorkerListVm : ListPanelVmBase<Worker>
    {
        public ICommand DeleteWorker
        { 
            get 
            { 
                return new DeleteEntityCommand<Worker>(entities => entities.Workers, (workers, id) => workers.Single(w => w.Id == (long)id), null); 
            } 
        }

        protected override Worker[] GetEntities()
        {
            Worker[] result;

            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                result = entities.Workers.Include(t => t.Machine).Where(w => w.Machine.SystemConfiguration.Description == ConfigurationSelection.SelectedConfiguration).ToArray();
            }

            return result;
        }
    }
}
