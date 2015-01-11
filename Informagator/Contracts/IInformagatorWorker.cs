﻿using Informagator.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts
{
    public interface IInformagatorWorker : IInformagatorRunner
    {
        ThreadConfiguration Configuration { set; }

        IList<string> RequiredAssemblies { get; }

        void ValidateSettings();
    }
}
