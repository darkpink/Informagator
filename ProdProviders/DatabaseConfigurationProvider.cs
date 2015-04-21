using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Informagator.Contracts;
using Informagator.Contracts.Configuration;
using Informagator.Contracts.Providers;
using Informagator.DBEntities.Configuration;

namespace Informagator.ProdProviders
{
    public class DatabaseConfigurationProvider : IConfigurationProvider
    {
        private Func<ConfigurationEntities, ICollection<Machine>> MachineQuery
        {
            get
            {
                return entities => entities
                    .SystemConfigurations
                    .Include(sc => sc.Assemblies)
                    .Include(sc => sc.Machines.Select(m => m.Workers
                                                            .Select(w => w.WorkerErrorHandlers
                                                                          .Select(weh => weh.ErrorHandler.ErrorHandlerParameters))))
                    .Include(av => av.Machines.Select(h => h.Workers
                                                            .Select(t => t.Stages
                                                                        .Select(s => s.StageParameters))))
                    .Include(av => av.Machines.Select(h => h.Workers
                                                            .Select(t => t.Stages
                                                                        .Select(s => s.StageErrorHandlers
                                                                                      .Select(seh => seh.ErrorHandler.ErrorHandlerParameters)))))
                    .Single(av => av.IsActive)
                    .Machines;
            }
        }
        public IMachineConfiguration GetMachineConfiguration(string hostName)
        {
            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                Machine result = GetMachineByName(hostName, entities);

                if (result == null)
                {
                    result = GetMachineByIP(hostName, entities);
                }

                return result;
            }
        }

        private Machine GetMachineByName(string hostName, ConfigurationEntities entities)
        {
            return MachineQuery(entities)
                   .SingleOrDefault(h => h.Name == hostName);
        }

        private Machine GetMachineByIP(string hostName, ConfigurationEntities entities)
        {
            Machine result = null;
            var ip = System.Net.Dns.GetHostAddresses(Dns.GetHostName())
                        .Where(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        .Select(a => a.ToString())
                        .ToArray();

            int candidateIpIndex = 0;
            while (result == null && candidateIpIndex < ip.Length)
            {
                result = MachineQuery(entities)
                         .SingleOrDefault(h => h.IPAddress == ip[candidateIpIndex]);
                candidateIpIndex++;
            }

            return result;
        }

        public IEnumerable<string> GetActiveMachineNames()
        {
            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                return entities.SystemConfigurations
                               .Include(sc => sc.Machines)
                               .Where(sc => sc.IsActive)
                               .SelectMany(sc => sc.Machines.Select(m => m.Name))
                               .ToArray();
            }
        }

        public IWorkerConfiguration GetThreadConfiguration(string machineName, string threadName)
        {
            return GetMachineConfiguration(machineName).Workers[threadName];
        }
    }
}
