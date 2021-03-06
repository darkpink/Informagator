﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts.Providers
{
    public interface IAssemblyProvider
    {
        byte[] GetAssemblyBinary(string assemblyName, string assemblyVersion);

        byte[] GetDebuggingSymbolBinary(string assemblyName, string assemblyVersion);
    }
}
