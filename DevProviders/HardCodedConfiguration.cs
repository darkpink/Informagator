using Acadian.Informagator.Configuration;
using Acadian.Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.DevProviders
{
    public class HardCodedConfigurationProvider : IConfigurationProvider
    {
        bool first = true;
        public IInformagatorConfiguration Configuration
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
                    IDictionary<string, ThreadConfiguration> uncastedStageConfig = config.ThreadConfiguration;
                    ThreadConfiguration un = uncastedStageConfig["Demo"];
                    ThreadConfiguration casted = (ThreadConfiguration)un;
                    StageConfiguration stageConfig = casted.StageConfigurations[1] as StageConfiguration;
                    //StageConfigurationParameter configParam = stageConfig.Parameters.First() as StageConfigurationParameter;
                    //configParam.Value = @"E:\tst\Dest2";
                    return config;
                }
            }
        }
    }
    
    public class HardCodedConfiguration : IInformagatorConfiguration
    {
        private Dictionary<string, ThreadConfiguration> config {get; set;}
        public HardCodedConfiguration()
        {
            config = new Dictionary<string, ThreadConfiguration>() { 
                    {
                        "Demo", 
                        new ThreadConfiguration() {
                            Name = "Demo",
                            ThreadHostTypeAssembly = "Acadian.Informagator.dll",
                            ThreadHostTypeName = "Acadian.Informagator.Threads.ThreadHost",
                            WorkerClassTypeAssembly = "Acadian.Informagator.dll",
                            WorkerClassTypeName = "Acadian.Informagator.Threads.PollingStageWorker",
                            RequiredAssemblies = new[] {"Acadian.Informagator.CommonComponents.dll", "Acadian.Informagator.dll" }.ToList(),
                            StageConfigurations = new[] { new StageConfiguration()
                                                          { 
                                                              StageType = "Acadian.Informagator.CommonComponents.SupplierStages.OldestFileFromFolderSupplier",
                                                              //StageType = "Acadian.Informagator.CommonComponents.SupplierStages.MessageStoreSupplier",
                                                              StageAssemblyName = "Acadian.Informagator.CommonComponents.dll",
                                                              ErrorHandlerAssemblyName = "Acadian.Informagator.CommonComponents.dll",
                                                              ErrorHandlerType = "Acadian.Informagator.CommonComponents.ErrorHandlers.IgnoreErrorHandler",
                                                              Parameters = new[] { new StageConfigurationParameter() {
                                                                 Name = "FolderPath",
                                                                 Value = @"C:\Demo\Source"}}
                                                                 //Name = "QueueName",
                                                                 //Value = @"Demo"}}
                                                          },
                                                          new StageConfiguration()
                                                          { 
                                                              //StageType = "Acadian.Informagator.CommonComponents.ConsumerStages.OutputFolderConsumer",
                                                              StageType = "SandboxCustom.AlternatingFolderTransform",
                                                              StageAssemblyName = "SandboxCustom.dll",
                                                              ErrorHandlerAssemblyName = "Acadian.Informagator.CommonComponents.dll",
                                                              ErrorHandlerType = "Acadian.Informagator.CommonComponents.ErrorHandlers.IgnoreErrorHandler"
                                                          },
                                                          new StageConfiguration()
                                                          { 
                                                              //StageType = "Acadian.Informagator.CommonComponents.ConsumerStages.OutputFolderConsumer",
                                                              StageType = "Acadian.Informagator.CommonComponents.ConsumerStages.DynamicOutputFolderConsumer",
                                                              StageAssemblyName = "Acadian.Informagator.CommonComponents.dll",
                                                              ErrorHandlerAssemblyName = "Acadian.Informagator.CommonComponents.dll",
                                                              ErrorHandlerType = "Acadian.Informagator.CommonComponents.ErrorHandlers.IgnoreErrorHandler",
                                                              Parameters = new[] { new StageConfigurationParameter() {
                                                                 Name = "FolderPathAttribute",
                                                                 Value = @"FolderName"}}
                                                          }
                                                        }.ToList()
                        }
                    } };

        }

        public string ProcessName
        {
            get { return "LocalInformagator"; }
        }

        public Dictionary<string, ThreadConfiguration> ThreadConfiguration
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
