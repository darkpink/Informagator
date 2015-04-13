using Informagator.Contracts;
using Informagator.Contracts.Configuration;
using Informagator.Contracts.Providers;
using Informagator.DevProviders.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders
{
    public class HardCodedConfigurationProvider : IConfigurationProvider
    {
        bool first = true;

        public string MachineName { protected get; set; }

        public IMachineConfiguration GetMachineConfiguration(string machineName)
        {
            if (first)
            {
                first = false;
                return new HardCodedConfiguration();
            }
            else
            {
                HardCodedConfiguration config = new HardCodedConfiguration();
                IDictionary<string, IWorkerConfiguration> uncastedStageConfig = config.Workers;
                IWorkerConfiguration un = uncastedStageConfig["FileMover"];
                HardCodedThreadConfiguration casted = (HardCodedThreadConfiguration)un;
                HardCodedStageConfiguration stageConfig = casted.Stages[1] as HardCodedStageConfiguration;
                //StageConfigurationParameter configParam = stageConfig.Parameters.First() as StageConfigurationParameter;
                //configParam.Value = @"E:\tst\Dest2";
                return config;
            }
        }

        public IEnumerable<string> GetActiveMachineNames()
        {
            throw new NotImplementedException();
        }

        public IWorkerConfiguration GetThreadConfiguration(string machineName, string threadName)
        {
            return new HardCodedConfiguration().Workers[threadName];
        }
    }
    
    public class HardCodedConfiguration : IMachineConfiguration
    {
        private IDictionary<string, IWorkerConfiguration> config {get; set;}
        public HardCodedConfiguration()
        {
            config = new Dictionary<string, IWorkerConfiguration>() { 
                    {
                        "FileMover", 
                        new HardCodedThreadConfiguration() {
                            Name = "FileMover",
                            AssemblyName = "Informagator.CommonComponents.dll",
                            Type = "Informagator.CommonComponents.Workers.PollingStageWorker",
                            Stages = new[] { new HardCodedStageConfiguration()
                                                          { 
                                                              Type = "Informagator.CommonComponents.SupplierStages.OldestFileFromFolderSupplier",
                                                              //StageType = "Informagator.CommonComponents.SupplierStages.MessageStoreSupplier",
                                                              AssemblyName = "Informagator.CommonComponents.dll",
                                                              Parameters = new[] { new HardCodedStageConfigurationParameter() {
                                                                 Name = "FolderPath",
                                                                 Value = @"C:\Demo\In"}}
                                                                 //Name = "QueueName",
                                                                 //Value = @"Demo"}}
                                                          },
                                                          new HardCodedStageConfiguration()
                                                          { 
                                                              //StageType = "Informagator.CommonComponents.ConsumerStages.OutputFolderConsumer",
                                                              Type = "Informagator.CommonComponents.ConsumerStages.StaticOutputFolderConsumer",
                                                              AssemblyName = "Informagator.CommonComponents.dll",
                                                              Parameters = new[] { new HardCodedStageConfigurationParameter() {
                                                                 Name = "FolderPath",
                                                                 Value = @"C:\Demo\Out"}}
                                                          }
                                                        }.OfType<IStageConfiguration>().ToList()
                        }
                    } };

        }

        public string HostName
        {
            get { return "LocalInformagator"; }
        }

        public string IPAddress { get { return "127.0.0.1"; } }

        public IDictionary<string, IWorkerConfiguration> Workers
        {
            get 
            {
                return config;
            }
        }


        public int AdminServicePort
        {
            get { return 9001; }
        }

        public string AdminServiceGroup
        {
            get { return @"DOM\AdminServiceUsers"; }
        }

        public int InfoServicePort
        {
            get { return 9002; }
        }

        public string InfoServiceGroup
        {
            get { return @"DOM\Everyone"; }
        }


        public bool SuppressParentErrorHandlers { get; set; }
    }
}
