using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
using Informagator.Contracts;
using Informagator.Contracts.Attributes;
using Informagator.Contracts.Stages;
using Informagator.Contracts.Configuration;
using Informagator.Contracts.WorkerServices;

namespace Informagator.CommonComponents.Workers
{
    public class PollingStageWorker : IntervalExecutionThread
    {
        [HostProvided]
        public IAssemblyManager AssemblyManager { get; set; }
        protected ProcessingSequence Stages { get; set; }

        protected override void OnInitialize()
        {
            BuildStages();
        }
        public void BuildStages()
        {
            Stages = new ProcessingSequence();
            Stages.MessageTracker = MessageTracker;
            
            foreach (IStageConfiguration stageConfig in Configuration.StageConfigurations)
            {
                //TODO create something in AssemblyManager like GetType();
                Assembly stageTypeAssembly = AssemblyManager.GetAssembly(stageConfig.StageAssemblyName);
                Type stageType = stageTypeAssembly.GetType(stageConfig.StageType);
                
                Assembly errorHandlerAssembly = AssemblyManager.GetAssembly(stageConfig.ErrorHandlerAssemblyName);
                Type errorHandlerType = errorHandlerAssembly.GetType(stageConfig.ErrorHandlerType);
                IMessageErrorHandler errorHandler = Activator.CreateInstance(errorHandlerType) as IMessageErrorHandler;

                IProcessingStage stage = (IProcessingStage)(Activator.CreateInstance(stageType));

                var configProps = stageType.GetProperties().Where(pi => pi.GetCustomAttributes().OfType<ConfigurationParameterAttribute>().Any());
                foreach (PropertyInfo pi in configProps)
                {
                    ConfigurationParameterAttribute[] configParams = pi.GetCustomAttributes().OfType<ConfigurationParameterAttribute>().ToArray();
                    IStageConfigurationParameter param = stageConfig.Parameters.SingleOrDefault(p => p.Name == pi.Name);
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
            bool result = Stages.Execute();
            
            if (result)
            {
                LastMessage = DateTime.Now;
                Info = "Last message received at " + LastMessage.Value.ToString("MM/dd/yyyy HH:mm:ss");
                MessageCount++;
            }

            return result;
        }
    }
}
