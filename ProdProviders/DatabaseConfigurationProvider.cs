using Acadian.Informagator.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

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

                using (InformagatorEntities entities = new InformagatorEntities())
                {
                    Host dbHostEntity = entities.Hosts.Include(h => h.Threads
                                                                     .Select(t => t.Stages
                                                                                   .Select(s => s.StageParameters)))
                        .SingleOrDefault(h => h.Name == hostName);
                    if (dbHostEntity != null)
                    {
                        foreach (Thread t in dbHostEntity.Threads)
                        {
                            ThreadConfiguration threadConfig = new ThreadConfiguration();
                            threadConfig.Name = t.Name;
                            threadConfig.ThreadHostTypeAssembly = t.WorkerAssembly;
                            threadConfig.ThreadHostTypeName = t.WorkerType;
                            threadConfig.RequiredAssemblies.Add(t.WorkerAssembly);

                            foreach (Stage s in t.Stages.OrderBy(s => s.Sequence))
                            {
                                StageConfiguration stageConfig = new StageConfiguration();
                                stageConfig.StageAssemblyName = s.StageAssembly;
                                stageConfig.StageType = s.StageType;
                                stageConfig.ErrorHandlerAssemblyName = s.ErrorHandlerAssembly;
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

                            config.ThreadConfiguration.Add(config.ProcessName, threadConfig);
                        }
                    }
                }

                return config;
            }
        }
    }
}
