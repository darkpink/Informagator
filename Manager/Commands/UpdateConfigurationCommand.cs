using Informagator.Contracts.Services;
using Informagator.DBEntities.Configuration;
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

                foreach (var machine in entities.Machines.Where(m => m.SystemConfiguration.IsActive))
                {
                    int portNumber = machine.AdminServicePort;
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
