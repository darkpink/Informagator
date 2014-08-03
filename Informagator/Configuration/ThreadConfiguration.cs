using Acadian.Informagator.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Configuration
{
    [Serializable]
    public class ThreadConfiguration : IThreadIsolatorConfiguration
    {
        public ThreadConfiguration()
        {
            RequiredAssemblies = new List<string>();
            StageConfigurations = new List<IStageConfiguration>();
        }
        
        public IList<IStageConfiguration> StageConfigurations { get; set; }

        public IList<string> RequiredAssemblies { get; set; }
        
        public string Name { get; set; }
        
        public string ThreadHostTypeAssembly { get; set; }
        
        public string ThreadHostTypeName { get; set; }
        
        public string WorkerClassTypeAssembly { get; set; }
        
        public string WorkerClassTypeName { get; set; }
        public bool IsSameAs(IThreadHostConfiguration config)
        {
            bool result = true;
            ThreadConfiguration castedConfig = config as ThreadConfiguration;
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

        protected bool HasCustomConfigurationDifferences(Informagator.Infrastructure.IThreadHostConfiguration config)
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


        public bool IsSameAs(IThreadIsolatorConfiguration config)
        {
            throw new NotImplementedException();
        }
    }
}
