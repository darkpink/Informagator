using Informagator.Contracts.Services;
using Informagator.ProdProviders.Configuration;
using Informagator.SystemStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Informagator.Manager.Commands
{
    public class UpdateConfigurationCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public void Execute(object parameter)
        {
            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                int portNumber = Int32.Parse(entities.GlobalSettings.Where(gs => gs.SystemConfiguration.IsActive && gs.Name == "AdminServicePort").SingleOrDefault().Value);

                foreach (var machine in entities.Machines.Where(m => m.SystemConfiguration.IsActive))
                {
                    string machineName = machine.Name;
                    if (!String.IsNullOrWhiteSpace(machine.IPAddress))
                    {
                        machineName = machine.IPAddress;
                    }

                    string url = AdminServiceAddress.Format(machineName, portNumber);
                    AdminServiceClient client = new AdminServiceClient(url);
                    client.UpdateConfiguration();
                }
            }
        }
    }
}
