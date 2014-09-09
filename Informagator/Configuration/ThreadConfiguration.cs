using Acadian.Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Configuration
{
    [Serializable]
    [DataContract]
    public class ThreadConfiguration
    {
        public ThreadConfiguration()
        {
            StageConfigurations = new List<StageConfiguration>();
        }

        public ThreadConfiguration(ThreadConfiguration configuration)
        {
            StageConfigurations = configuration.StageConfigurations.ToList();
        }

        [DataMember]
        public List<StageConfiguration> StageConfigurations { get; set; }


        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ThreadHostTypeAssembly { get; set; }

        [DataMember]
        public string ThreadHostTypeName { get; set; }

        [DataMember]
        public string WorkerClassTypeAssembly { get; set; }

        [DataMember]
        public string WorkerClassTypeName { get; set; }

        [DataMember]
        public List<string> RequiredAssemblies { get; set; }

        public bool IsSameAs(ThreadConfiguration config)
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

        protected bool HasCustomConfigurationDifferences(ThreadConfiguration config)
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
