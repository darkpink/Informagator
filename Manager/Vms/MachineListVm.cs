using Informagator.DBEntities.Configuration;
using Informagator.Manager.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Informagator.Manager.Vms
{
    public class MachineListVm : ListPanelVmBase<Informagator.DBEntities.Configuration.Machine>
    {
        public ICommand DeleteMachine
        {
            get
            {
                return new DeleteEntityCommand<Machine>(entities => entities.Machines, (Machines, id) => Machines.Single(w => w.Id == (long)id), DeleteErrorHandlers, Refresh);
            }
        }

        protected void DeleteErrorHandlers(Machine machine)
        {
            using(ConfigurationEntities entities = new ConfigurationEntities())
            {
                foreach(long id in machine.MachineErrorHandlers.Select(eh => eh.Id).ToList())
                {
                    MachineErrorHandler toRemove = entities.MachineErrorHandlers.Single(eh => eh.Id == id);
                    entities.MachineErrorHandlers.Remove(toRemove);
                }

                entities.SaveChanges();
            }
        }

        protected override Informagator.DBEntities.Configuration.Machine[] GetEntities()
        {
            Informagator.DBEntities.Configuration.Machine[] result;

            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                result = entities.Machines.Where(m => m.SystemConfiguration.Name == ConfigurationSelection.SelectedConfiguration).ToArray();
            }

            return result;
        }
    }
}
