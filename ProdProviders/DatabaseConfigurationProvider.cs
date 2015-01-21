using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Informagator.Contracts;
using Informagator.ProdProviders.Configuration;
using Informagator.Contracts.Configuration;
using Informagator.Contracts.Providers;

namespace Informagator.ProdProviders
{
    public class DatabaseConfigurationProvider : IConfigurationProvider
    {
        public IMachineConfiguration GetMachineConfiguration(string hostName)
        {
            DatabaseInformagatorConfiguration config = null;

            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                var dbHostEntity = GetMachineEntity(hostName, entities);
                config = new DatabaseInformagatorConfiguration(hostName, dbHostEntity.IPAddress);
                
                if (dbHostEntity != null)
                {
                    foreach (Worker t in dbHostEntity.Workers)
                    {
                        DatabaseThreadConfiguration threadConfig = new DatabaseThreadConfiguration();
                        threadConfig.Name = t.Name;
                        threadConfig.WorkerClassTypeAssembly = t.WorkerAssemblyVersion.AssemblyName;
                        threadConfig.WorkerClassTypeName = t.WorkerType;

                        foreach (Stage s in t.Stages.OrderBy(s => s.Sequence))
                        {
                            DatabaseStageConfiguration stageConfig = new DatabaseStageConfiguration();
                            stageConfig.StageAssemblyName = s.StageAssemblyVersion.AssemblyName;
                            stageConfig.StageType = s.StageType;
                            stageConfig.ErrorHandlerAssemblyName = s.ErrorHandlerAssemblyVersion.AssemblyName;
                            stageConfig.ErrorHandlerType = s.ErrorHandlerType;

                            foreach (StageParameter p in s.StageParameters)
                            {
                                DatabaseStageConfigurationParameter stageParam = new DatabaseStageConfigurationParameter();
                                stageParam.Name = p.Name;
                                stageParam.Value = p.Value;
                                stageConfig.Parameters.Add(stageParam);
                            }

                            threadConfig.StageConfigurations.Add(stageConfig);
                        }

                        config.ThreadConfiguration.Add(threadConfig.Name, threadConfig);
                    }
                }

                var activeconfig = entities.SystemConfigurations
                                             .Include(sc => sc.GlobalSettings)
                                             .Single(av => av.IsActive);
                config.AdminServicePort = Int32.Parse(activeconfig.GlobalSettings.Where(s => s.Name == "AdminServicePort").Select(s => s.Value).SingleOrDefault());
                config.InfoServicePort = Int32.Parse(activeconfig.GlobalSettings.Where(s => s.Name == "InfoServicePort").Select(s => s.Value).SingleOrDefault());
            }

            return config;
        }

        private static Informagator.ProdProviders.Configuration.Machine GetMachineEntity(string hostName, ConfigurationEntities entities)
        {
            var dbHostEntity = entities
                                .SystemConfigurations
                                .Include(av => av.Machines.Select(h => h.Workers
                                                                     .Select(t => t.Stages
                                                                                   .Select(s => s.StageParameters))))
                                .Single(av => av.IsActive)
                                .Machines
                                .SingleOrDefault(h => h.Name == hostName);

            if (dbHostEntity == null)
            {
                var ip = System.Net.Dns.GetHostAddresses(Dns.GetHostName())
                            .Where(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            .Select(a => a.ToString())
                            .ToArray();

                int candidateIpIndex = 0;
                while (dbHostEntity == null && candidateIpIndex < ip.Length)
                {
                    dbHostEntity = entities
                                    .SystemConfigurations
                                    .Include(av => av.Machines.Select(h => h.Workers
                                                                         .Select(t => t.Stages
                                                                                       .Select(s => s.StageParameters))))
                                    .Single(av => av.IsActive)
                                    .Machines
                                    .SingleOrDefault(h => h.IPAddress == ip[candidateIpIndex]);
                    candidateIpIndex++;
                }
            }

            if (dbHostEntity == null)
            {
                dbHostEntity = entities
                                .SystemConfigurations
                                .Include(av => av.Machines.Select(h => h.Workers
                                                                     .Select(t => t.Stages
                                                                                   .Select(s => s.StageParameters))))
                                .Single(av => av.IsActive)
                                .Machines
                                .SingleOrDefault(h => h.IPAddress == IPAddress.Loopback.ToString());
            }

            return dbHostEntity;
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

        public IThreadConfiguration GetThreadConfiguration(string machineName, string threadName)
        {
            return GetMachineConfiguration(machineName).ThreadConfiguration[threadName];
        }
    }
}
