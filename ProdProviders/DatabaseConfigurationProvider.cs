using Acadian.Informagator.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Acadian.Informagator.Contracts;
using Acadian.Informagator.ProdProviders.Configuration;

namespace Acadian.Informagator.ProdProviders
{
    public class DatabaseConfigurationProvider : IConfigurationProvider
    {
        public IInformagatorConfiguration Configuration
        {
            get
            {
                string hostName = Dns.GetHostName();
                InformagatorConfiguration config = new InformagatorConfiguration(hostName);

                using (ConfigurationEntities entities = new ConfigurationEntities())
                {
                    var dbHostEntity = entities
                                        .SystemConfigurations
                                        .Include(av => av.Machines.Select(h => h.Workers
                                                                             .Select(t => t.Stages
                                                                                           .Select(s => s.StageParameters))))
                                        .Single(av => av.IsActive)
                                        .Machines
                                        .Single(h => h.Name == hostName);

                    if (dbHostEntity != null)
                    {
                        foreach (Worker t in dbHostEntity.Workers)
                        {
                            ThreadConfiguration threadConfig = new ThreadConfiguration();
                            threadConfig.Name = t.Name;
                            threadConfig.ThreadHostTypeAssembly = t.WorkerAssemblyVersion.AssemblyName;
                            threadConfig.ThreadHostTypeName = t.WorkerType;
                            threadConfig.RequiredAssemblies.Add(t.WorkerAssemblyVersion.AssemblyName);

                            foreach (Stage s in t.Stages.OrderBy(s => s.Sequence))
                            {
                                StageConfiguration stageConfig = new StageConfiguration();
                                stageConfig.StageAssemblyName = s.StageAssemblyVersion.AssemblyName;
                                stageConfig.StageType = s.StageType;
                                stageConfig.ErrorHandlerAssemblyName = s.ErrorHandlerAssemblyVersion.AssemblyName;
                                stageConfig.ErrorHandlerType = s.ErrorHandlerType;
                                threadConfig.RequiredAssemblies.Add(stageConfig.ErrorHandlerAssemblyName);
                                threadConfig.RequiredAssemblies.Add(stageConfig.StageAssemblyName);

                                foreach (StageParameter p in s.StageParameters)
                                {
                                    StageConfigurationParameter stageParam = new StageConfigurationParameter();
                                    stageParam.Name = p.Name;
                                    stageParam.Value = p.Value;
                                    stageConfig.Parameters.Add(stageParam);
                                }

                                threadConfig.StageConfigurations.Add(stageConfig);
                            }

                            config.ThreadConfiguration.Add(threadConfig.Name, threadConfig);
                        }
                    }
                }

                return config;
            }
        }
    }
}
