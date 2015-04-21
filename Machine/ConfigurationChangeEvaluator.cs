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
        public bool IsRestartRequired(HostIsolator thread, IWorkerConfiguration oldConfiguration, IWorkerConfiguration newConfiguration)
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

        protected bool AreConfigurationsIdentical(IWorkerConfiguration config1, IWorkerConfiguration config2)
        {
            bool result = true;

            result &= config1.AssemblyName == config2.AssemblyName;
            result &= config1.AssemblyVersion == config2.AssemblyVersion;
            result &= config1.Type == config2.Type;
            result &= config1.Name == config2.Name;

            result &= AreWorkerConfigurationParametersIdentical(config1.Parameters, config2.Parameters);
            result &= AreStageConfigurationsEqual(config1.Stages, config2.Stages);
            result &= AreErrorHandlersIdentical(config1.ErrorHandlers, config2.ErrorHandlers);

            return result;
        }

        private bool AreWorkerConfigurationParametersIdentical(IList<IConfigurationParameter> list1, IList<IConfigurationParameter> list2)
        {
            bool result = true;

            list1 = list1 ?? new List<IConfigurationParameter>();
            list2 = list2 ?? new List<IConfigurationParameter>();

            result &= list1.Count() == list2.Count;

            foreach (IConfigurationParameter config1Param in list1)
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
                result &= list2.Count(config2Param => config2Param.SuppressParentErrorHandlers == config1Param.SuppressParentErrorHandlers &&
                                                      config2Param.AssemblyName == config1Param.AssemblyName &&
                                                      config2Param.Type == config1Param.Type &&
                                                      config1Param.AssemblyVersion == config2Param.AssemblyVersion &&
                                                      AreConfigurationParametersIdentical(config1Param.Parameters, config2Param.Parameters) &&
                                                      AreErrorHandlersIdentical(config1Param.ErrorHandlers, config2Param.ErrorHandlers)
                                     ) == 1;
            }

            return result;
        }

        protected bool AreErrorHandlersIdentical(IList<IErrorHandlerConfiguration> list1, IList<IErrorHandlerConfiguration> list2)
        {
            bool result = true;

            list1 = list1 ?? new List<IErrorHandlerConfiguration>();
            list2 = list2 ?? new List<IErrorHandlerConfiguration>();

            result &= list1.Count == list2.Count;

            foreach (IErrorHandlerConfiguration errorHandler1 in list1)
            {
                result &= list2.Count(errorHandler2 => errorHandler2.AssemblyName == errorHandler1.AssemblyName &&
                                                   errorHandler2.Type == errorHandler1.Type &&
                                                   errorHandler1.AssemblyVersion == errorHandler2.AssemblyVersion &&
                                                   AreConfigurationParametersIdentical(errorHandler1.Parameters, errorHandler2.Parameters)
                                  ) == 1;
            }

            return result;
        }

        private bool AreConfigurationParametersIdentical(IList<IConfigurationParameter> list1, IList<IConfigurationParameter> list2)
        {
            bool result = true;

            list1 = list1 ?? new List<IConfigurationParameter>();
            list2 = list2 ?? new List<IConfigurationParameter>();

            result &= list1.Count() == list2.Count;

            foreach (IConfigurationParameter config1Param in list1)
            {
                result &= list2.Count(config2Param => config2Param.Name == config1Param.Name && config2Param.Value == config1Param.Value) == 1;
            }

            return result;
        }
    }
}
