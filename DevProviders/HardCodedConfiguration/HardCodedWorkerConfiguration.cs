﻿using Informagator.Contracts;
using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders.HardCodedConfiguration
{
    [Serializable]
    public class HardCodedWorkerConfiguration : IWorkerConfiguration
    {
        public HardCodedWorkerConfiguration()
        {
            Stages = new List<IStageConfiguration>();
        }

        public HardCodedWorkerConfiguration(HardCodedWorkerConfiguration configuration)
        {
            Stages = configuration.Stages.ToList();
        }

        public IList<IStageConfiguration> Stages { get; set; }

        public string Name { get; set; }

        public string AssemblyName { get; set; }
        
        public string AssemblyVersion { get; set; }

        public string Type { get; set; }

        public bool AutoStart { get; set; }

        public IList<IConfigurationParameter> Parameters
        {
            get { return new List<IConfigurationParameter>(); }
        }


        public IList<IErrorHandlerConfiguration> ErrorHandlers
        {
            get { return new List<IErrorHandlerConfiguration>(); }
        }


        public bool SuppressParentErrorHandlers { get; set; }
    }
}
