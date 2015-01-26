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
    public class StartThreadCommand : ICommand
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
            string machineName = ((AutoRefreshingThreadStatus)parameter).MachineName;
            string threadName = ((AutoRefreshingThreadStatus)parameter).ThreadName;
            int portNumber;

            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                var machine = entities.Machines.Where(m => m.SystemConfiguration.IsActive && m.Name == machineName).Single();
                if (!String.IsNullOrWhiteSpace(machine.IPAddress))
                {
                    machineName = machine.IPAddress;
                }

                portNumber = machine.AdminServicePort;
            }


            string url = AdminServiceAddress.Format(machineName, portNumber);
            AdminServiceClient client = new AdminServiceClient(url);
            client.StartThread(threadName);
        }
    }
}
