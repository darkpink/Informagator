using Informagator.Contracts;
using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders.Configuration
{
    [Serializable]
    public class HardCodedThreadConfiguration : IThreadConfiguration
    {
        public HardCodedThreadConfiguration()
        {
            StageConfigurations = new List<IStageConfiguration>();
        }

        public HardCodedThreadConfiguration(HardCodedThreadConfiguration configuration)
        {
            StageConfigurations = configuration.StageConfigurations.ToList();
        }

        public IList<IStageConfiguration> StageConfigurations { get; set; }


        public string Name { get; set; }

        public string ThreadHostTypeAssembly { get; set; }

        public string ThreadHostTypeName { get; set; }

        public string WorkerClassTypeAssembly { get; set; }

        public string WorkerClassTypeName { get; set; }

        public IList<string> RequiredAssemblies { get; set; }

        public bool IsSameAs(IThreadConfiguration config)
        {
            bool result = true;
            HardCodedThreadConfiguration castedConfig = config as HardCodedThreadConfiguration;
            if (castedConfig == null)
            {
                result = false;
            }
            else
            {
                result &= Name == castedConfig.Name;
                result &= ThreadHostTypeAssembly == castedConfig.ThreadHostTypeAssembly;
                result &= ThreadHostTypeName == castedConfig.ThreadHostTypeName;
                result &= WorkerClassTypeAssembly == castedConfig.WorkerClassTypeAssembly;
                result &= WorkerClassTypeName == castedConfig.WorkerClassTypeName;

                foreach (string required in RequiredAssemblies)
                {
                    result &= castedConfig.RequiredAssemblies.Contains(required);
                }

                foreach (string required in castedConfig.RequiredAssemblies)
                {
                    result &= RequiredAssemblies.Contains(required);
                }

                result = result && !HasCustomConfigurationDifferences(castedConfig);
            }

            return result;
        }

        protected bool HasCustomConfigurationDifferences(IThreadConfiguration config)
        {
            bool result;
            
            if (config is HardCodedThreadConfiguration)
            {
                result = false;
                HardCodedThreadConfiguration castedConfig = (HardCodedThreadConfiguration)config;
                if (StageConfigurations.Count == castedConfig.StageConfigurations.Count)
                {
                    for (int stageIndex = 0; stageIndex < StageConfigurations.Count; stageIndex++)
                    {
                        IStageConfiguration c1 = StageConfigurations[stageIndex];
                        IStageConfiguration c2 = castedConfig.StageConfigurations[stageIndex];
                        result |= !c1.IsSameAs(c2);
                    }
                }
                else
                {
                    result = true;
                }
            }
            else
            {
                result = true;
            }
            
            return result;
        }
    }
}
