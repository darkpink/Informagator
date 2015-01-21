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
                IDictionary<string, IThreadConfiguration> uncastedStageConfig = config.ThreadConfiguration;
                IThreadConfiguration un = uncastedStageConfig["FileMover"];
                HardCodedThreadConfiguration casted = (HardCodedThreadConfiguration)un;
                HardCodedStageConfiguration stageConfig = casted.StageConfigurations[1] as HardCodedStageConfiguration;
                //StageConfigurationParameter configParam = stageConfig.Parameters.First() as StageConfigurationParameter;
                //configParam.Value = @"E:\tst\Dest2";
                return config;
            }
        }

        public IEnumerable<string> GetActiveMachineNames()
        {
            throw new NotImplementedException();
        }

        public IThreadConfiguration GetThreadConfiguration(string machineName, string threadName)
        {
            return new HardCodedConfiguration().ThreadConfiguration[threadName];
        }
    }
    
    public class HardCodedConfiguration : IMachineConfiguration
    {
        private IDictionary<string, IThreadConfiguration> config {get; set;}
        public HardCodedConfiguration()
        {
            config = new Dictionary<string, IThreadConfiguration>() { 
                    {
                        "FileMover", 
                        new HardCodedThreadConfiguration() {
                            Name = "FileMover",
                            WorkerClassTypeAssembly = "Informagator.CommonComponents.dll",
                            WorkerClassTypeName = "Informagator.CommonComponents.Workers.PollingStageWorker",
                            StageConfigurations = new[] { new HardCodedStageConfiguration()
                                                          { 
                                                              StageType = "Informagator.CommonComponents.SupplierStages.OldestFileFromFolderSupplier",
                                                              //StageType = "Informagator.CommonComponents.SupplierStages.MessageStoreSupplier",
                                                              StageAssemblyName = "Informagator.CommonComponents.dll",
                                                              ErrorHandlerAssemblyName = "Informagator.CommonComponents.dll",
                                                              ErrorHandlerType = "Informagator.CommonComponents.ErrorHandlers.IgnoreErrorHandler",
                                                              Parameters = new[] { new HardCodedStageConfigurationParameter() {
                                                                 Name = "FolderPath",
                                                                 Value = @"C:\Demo\In"}}
                                                                 //Name = "QueueName",
                                                                 //Value = @"Demo"}}
                                                          },
                                                          new HardCodedStageConfiguration()
                                                          { 
                                                              //StageType = "Informagator.CommonComponents.ConsumerStages.OutputFolderConsumer",
                                                              StageType = "Informagator.CommonComponents.ConsumerStages.StaticOutputFolderConsumer",
                                                              StageAssemblyName = "Informagator.CommonComponents.dll",
                                                              ErrorHandlerAssemblyName = "Informagator.CommonComponents.dll",
                                                              ErrorHandlerType = "Informagator.CommonComponents.ErrorHandlers.IgnoreErrorHandler",
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

        public IDictionary<string, IThreadConfiguration> ThreadConfiguration
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
    }
}
