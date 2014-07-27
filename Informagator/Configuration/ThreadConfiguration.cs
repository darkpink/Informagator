using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Configuration
{
    [Serializable]
    public class ThreadConfiguration : IThreadConfiguration
    {
        public ThreadConfiguration()
        {
            RequiredAssemblies = new List<string>();
            StageConfigurations = new List<StageConfiguration>();
        }
        
        public IList<StageConfiguration> StageConfigurations { get; set; }

        public IList<string> RequiredAssemblies { get; set; }
        
        public string Name { get; set; }
        
        public string ThreadHostTypeAssembly { get; set; }
        
        public string ThreadHostTypeName { get; set; }
        
        public string WorkerClassTypeAssembly { get; set; }
        
        public string WorkerClassTypeName { get; set; }
        
        public bool IsSameAs(IThreadConfiguration config)
        {
            bool result = true;

            result &= Name == config.Name;
            result &= ThreadHostTypeAssembly == config.ThreadHostTypeAssembly;
            result &= ThreadHostTypeName == config.ThreadHostTypeName;
            result &= WorkerClassTypeAssembly == config.WorkerClassTypeAssembly;
            result &= WorkerClassTypeName == config.WorkerClassTypeName;

            foreach (string required in RequiredAssemblies)
            {
                result &= config.RequiredAssemblies.Contains(required);
            }

            foreach (string required in config.RequiredAssemblies)
            {
                result &= RequiredAssemblies.Contains(required);
            }

            result = result && !HasCustomConfigurationDifferences(config);
            return result;
        }

        protected bool HasCustomConfigurationDifferences(Informagator.Configuration.IThreadConfiguration config)
        {
            bool result;
            
            if (config is ThreadConfiguration)
            {
                result = false;
                ThreadConfiguration castedConfig = (ThreadConfiguration)config;
                if (StageConfigurations.Count == castedConfig.StageConfigurations.Count)
                {
                    for (int stageIndex = 0; stageIndex < StageConfigurations.Count; stageIndex++)
                    {
                        StageConfiguration c1 = StageConfigurations[stageIndex];
                        StageConfiguration c2 = castedConfig.StageConfigurations[stageIndex];
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
