using Acadian.Informagator.Configuration;
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
                    IDictionary<string, IThreadConfiguration> uncastedStageConfig = config.ThreadConfiguration;
                    IThreadConfiguration un = uncastedStageConfig["Demo"];
                    ThreadConfiguration casted = (ThreadConfiguration)un;
                    StageConfiguration stageConfig = casted.StageConfigurations[1];
                    StageConfigurationParameter configParam = stageConfig.Parameters.First();
                    configParam.Value = @"C:\Demo\SecondDestination";
                    return config;
                }
            }
        }
    }
    
    public class HardCodedConfiguration : IInformagatorConfiguration
    {
        private Dictionary<string, IThreadConfiguration> config {get; set;}
        public HardCodedConfiguration()
        {
            config = new Dictionary<string, IThreadConfiguration>() { 
                    {
                        "Demo", 
                        new ThreadConfiguration() {
                            Name = "Demo",
                            ThreadHostTypeAssembly = "Acadian.Informagator.dll",
                            ThreadHostTypeName = "Acadian.Informagator.Threads.PollingStageWorker",
                            RequiredAssemblies = new[] {"Acadian.Informagator.CommonComponents.dll", "Acadian.Informagator.dll" },
                            StageConfigurations = new[] { new StageConfiguration()
                                                          { 
                                                              StageType = "Acadian.Informagator.CommonComponents.SupplierStages.OldestFileFromFolderSupplier",
                                                              StageAssemblyName = "Acadian.Informagator.CommonComponents.dll",
                                                              ErrorHandlerAssemblyName = "Acadian.Informagator.CommonComponents.dll",
                                                              ErrorHandlerType = "Acadian.Informagator.CommonComponents.ErrorHandlers.IgnoreErrorHandler",
                                                              Parameters = new[] { new StageConfigurationParameter() {
                                                                 Name = "FolderPath",
                                                                 Value = @"C:\Demo\Source"}}
                                                          },
                                                          new StageConfiguration()
                                                          { 
                                                              StageType = "Acadian.Informagator.CommonComponents.ConsumerStages.OutputFolderConsumer",
                                                              StageAssemblyName = "Acadian.Informagator.CommonComponents.dll",
                                                              ErrorHandlerAssemblyName = "Acadian.Informagator.CommonComponents.dll",
                                                              ErrorHandlerType = "Acadian.Informagator.CommonComponents.ErrorHandlers.IgnoreErrorHandler",
                                                              Parameters = new[] { new StageConfigurationParameter() {
                                                                 Name = "FolderPath",
                                                                 Value = @"C:\Demo\Destination"}}
                                                          }
                                                        }
                        }
                    } };

        }

        public string ProcessName
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
