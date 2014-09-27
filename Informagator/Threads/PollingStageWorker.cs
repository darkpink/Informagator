using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acadian.Informagator.Configuration;
using Acadian.Informagator.Stages;
using System.Threading;
using System.Reflection;
using Acadian.Informagator.Threads;
using Acadian.Informagator.Contracts;

namespace Acadian.Informagator.Threads
{
    public class PollingStageWorker : IntervalExecutionThread
    {
        protected ProcessingSequence Stages { get; set; }

        protected override void OnInitialize()
        {
            BuildStages();
        }
        public void BuildStages()
        {
            Stages = new ProcessingSequence();
            Stages.MessageTracker = MessageTracker;
            
            foreach (StageConfiguration stageConfig in Configuration.StageConfigurations)
            {
                Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                Assembly stageTypeAssembly = loadedAssemblies.Single(a => a.ManifestModule.ScopeName == stageConfig.StageAssemblyName);
                Type stageType = stageTypeAssembly.GetType(stageConfig.StageType);
                
                Assembly errorHandlerAssembly = loadedAssemblies.Single(a => a.ManifestModule.ScopeName == stageConfig.ErrorHandlerAssemblyName);
                Type errorHandlerType = errorHandlerAssembly.GetType(stageConfig.ErrorHandlerType);
                IMessageErrorHandler errorHandler = Activator.CreateInstance(errorHandlerType) as IMessageErrorHandler;

                IProcessingStage stage = (IProcessingStage)(Activator.CreateInstance(stageType));

                var configProps = stageType.GetProperties().Where(pi => pi.GetCustomAttributes().OfType<ConfigurationParameterAttribute>().Any());
                foreach (PropertyInfo pi in configProps)
                {
                    ConfigurationParameterAttribute[] configParams = pi.GetCustomAttributes().OfType<ConfigurationParameterAttribute>().ToArray();
                    StageConfigurationParameter param = stageConfig.Parameters.SingleOrDefault(p => p.Name == pi.Name) as StageConfigurationParameter;
                    if (param != null)
                    {
                        pi.SetValue(stage, param.Value);
                    }
                }

                var dependencyProps = stageType.GetProperties().Where(pi => pi.GetCustomAttributes().OfType<HostProvidedAttribute>().Any());
                var myPropsForClients = this.GetType().GetProperties().Where(pi => pi.GetCustomAttributes().OfType<ProvideToClientAttribute>().Any());
                Dictionary<Type, object> availableDependencies = new Dictionary<Type, object>();
                foreach (PropertyInfo pi in myPropsForClients)
                {
                    var attrs = pi.GetCustomAttributes<ProvideToClientAttribute>();
                    var value = pi.GetValue(this);
                    foreach (ProvideToClientAttribute attr in attrs)
                    {
                        availableDependencies.Add(attr.InterfaceType, value);
                    }
                }

                foreach (PropertyInfo pi in dependencyProps)
                {
                    if (availableDependencies.ContainsKey(pi.PropertyType))
                    {
                        pi.SetValue(stage, availableDependencies[pi.PropertyType]);
                    }
                }

                Stages.Stages.Add(stage);
            }
        }
        protected override bool Execute()
        {
            return Stages.Execute();
        }
    }
}
