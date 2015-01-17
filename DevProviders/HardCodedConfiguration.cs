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

        public IMachineConfiguration Configuration
        {
            get
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
                            ThreadHostTypeAssembly = "Informagator.dll",
                            ThreadHostTypeName = "Informagator.Threads.ThreadHost",
                            WorkerClassTypeAssembly = "Informagator.dll",
                            WorkerClassTypeName = "Informagator.Threads.PollingStageWorker",
                            RequiredAssemblies = new[] {"Informagator.CommonComponents.dll", "Informagator.dll" }.ToList(),
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
                                                              StageType = "SandboxCustom.AlternatingFolderTransform",
                                                              StageAssemblyName = "SandboxCustom.dll",
                                                              ErrorHandlerAssemblyName = "Informagator.CommonComponents.dll",
                                                              ErrorHandlerType = "Informagator.CommonComponents.ErrorHandlers.IgnoreErrorHandler"
                                                          },
                                                          new HardCodedStageConfiguration()
                                                          { 
                                                              //StageType = "Informagator.CommonComponents.ConsumerStages.OutputFolderConsumer",
                                                              StageType = "Informagator.CommonComponents.ConsumerStages.DynamicOutputFolderConsumer",
                                                              StageAssemblyName = "Informagator.CommonComponents.dll",
                                                              ErrorHandlerAssemblyName = "Informagator.CommonComponents.dll",
                                                              ErrorHandlerType = "Informagator.CommonComponents.ErrorHandlers.IgnoreErrorHandler",
                                                              Parameters = new[] { new HardCodedStageConfigurationParameter() {
                                                                 Name = "FolderPathAttribute",
                                                                 Value = @"FolderName"}}
                                                          }
                                                        }.OfType<IStageConfiguration>().ToList()
                        }
                    } };

        }

        public string HostName
        {
            get { return "LocalInformagator"; }
        }

        public IDictionary<string, IThreadConfiguration> ThreadConfiguration
        {
            get 
            {
                return config;
            }
        }


        public System.Net.IPAddress AdminServiceAddress
        {
            get { return new IPAddress(new[]{(byte)127, (byte)0, (byte)0, (byte)1}); }
        }

        public int AdminServicePort
        {
            get { return 9449; }
        }

        public string AdminServiceGroup
        {
            get { return @"AMEDISYS-DOM\Users"; }
        }

        public System.Net.IPAddress InfoServiceAddress
        {
            get { return new IPAddress(new[] { (byte)127, (byte)0, (byte)0, (byte)1 }); }
        }

        public int InfoServicePort
        {
            get { return 9450; }
        }

        public string InfoServiceGroup
        {
            get { return @"AMEDISYS-DOM\Everyone"; }
        }
    }
}
