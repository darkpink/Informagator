using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Machine
{
    public class ConfigurationChangeEvaluator
    {
        public bool IsRestartRequired(HostIsolator thread, IThreadConfiguration oldConfiguration, IThreadConfiguration newConfiguration)
        {
            //The rules are: if the new config is the same as the old config and the new assemblies are the same as the old assemblies, then
            //no action is required.  If the assemblies changed, a thread rebuild is required.  If the assemblies are the same but the configuration
            //changed, then we ask the Worker

            if (thread.AnyAssemblyChanged)
            {
                return true;
            }
            else if (AreConfigurationsIdentical(oldConfiguration, newConfiguration))
            {
                return false;
            }
            else
            {
                return thread.IsRestartRequiredForNewConfiguration(newConfiguration);
            }

        }

        protected bool AreConfigurationsIdentical(IThreadConfiguration config1, IThreadConfiguration config2)
        {
            bool result = true;

            result &= config1.WorkerClassTypeAssembly == config2.WorkerClassTypeAssembly;
            result &= config1.WorkerClassTypeName == config2.WorkerClassTypeName;
            result &= config1.Name == config2.Name;

            result &= AreWorkerConfigurationParametersIdentical(config1.WorkerConfigurationParameters, config2.WorkerConfigurationParameters);
            result &= AreStageConfigurationsEqual(config1.StageConfigurations, config2.StageConfigurations);

            return result;
        }

        private bool AreWorkerConfigurationParametersIdentical(IList<IWorkerConfigurationParameter> list1, IList<IWorkerConfigurationParameter> list2)
        {
            bool result = true;

            list1 = list1 ?? new List<IWorkerConfigurationParameter>();
            list2 = list2 ?? new List<IWorkerConfigurationParameter>();

            result &= list1.Count() == list2.Count;

            foreach (IWorkerConfigurationParameter config1Param in list1)
            {
                result &= list2.Count(config2Param => config2Param.Name == config1Param.Name && config2Param.Value == config1Param.Value) == 1;
            }

            return result;
        }

        private bool AreStageConfigurationsEqual(IList<IStageConfiguration> list1, IList<IStageConfiguration> list2)
        {
            bool result = true;

            list1 = list1 ?? new List<IStageConfiguration>();
            list2 = list2 ?? new List<IStageConfiguration>();

            result &= list1.Count() == list2.Count;

            foreach (IStageConfiguration config1Param in list1)
            {
                result &= list2.Count(config2Param => config2Param.StageAssemblyName == config1Param.StageAssemblyName &&
                                                      config2Param.StageType == config1Param.StageType &&
                                                      config1Param.ErrorHandlerAssemblyName == config2Param.ErrorHandlerAssemblyName &&
                                                      config1Param.ErrorHandlerType == config2Param.ErrorHandlerType &&
                                                      AreStageConfigurationParametersIdentical(config1Param.Parameters, config2Param.Parameters)) == 1;
            }

            return result;
        }

        private bool AreStageConfigurationParametersIdentical(IList<IStageConfigurationParameter> list1, IList<IStageConfigurationParameter> list2)
        {
            bool result = true;

            list1 = list1 ?? new List<IStageConfigurationParameter>();
            list2 = list2 ?? new List<IStageConfigurationParameter>();

            result &= list1.Count() == list2.Count;

            foreach (IStageConfigurationParameter config1Param in list1)
            {
                result &= list2.Count(config2Param => config2Param.Name == config1Param.Name && config2Param.Value == config1Param.Value) == 1;
            }

            return result;
        }

    }
}
