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
using Acadian.Informagator.Infrastructure;

namespace Acadian.Informagator.Threads
{
    public class PollingStageWorker : IntervalExecutionThread
    {
        protected ProcessingSequence Stages { get; set; }
        public PollingStageWorker(IThreadConfiguration configuration)
            :base((ThreadConfiguration)configuration)
        {
            Stages = new ProcessingSequence();
            foreach (StageConfiguration stageConfig in Configuration.StageConfigurations)
            {
                Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                Assembly stageTypeAssembly = loadedAssemblies.Single(a => a.ManifestModule.ScopeName == stageConfig.StageAssemblyName);
                Type stageType = stageTypeAssembly.GetType(stageConfig.StageType);
                
                Assembly errorHandlerAssembly = loadedAssemblies.Single(a => a.ManifestModule.ScopeName == stageConfig.ErrorHandlerAssemblyName);
                Type errorHandlerType = errorHandlerAssembly.GetType(stageConfig.ErrorHandlerType);
                IMessageErrorHandler errorHandler = Activator.CreateInstance(errorHandlerType) as IMessageErrorHandler;

                ConstructorInfo stageConstructor = stageType.GetConstructor(new[] {typeof(IMessageErrorHandler)});
                ProcessingStage stage = (ProcessingStage)stageConstructor.Invoke(new object[]{errorHandler});

                var configProps = stageType.GetProperties().Where(pi => pi.GetCustomAttributes().OfType<ConfigurationParameterAttribute>().Any());
                foreach (PropertyInfo pi in configProps)
                {
                    ConfigurationParameterAttribute[] configParams = pi.GetCustomAttributes().OfType<ConfigurationParameterAttribute>().ToArray();
                    StageConfigurationParameter param = stageConfig.Parameters.SingleOrDefault(p => p.Name == pi.Name);
                    if (param != null)
                    {
                        pi.SetValue(stage, param.Value);
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
