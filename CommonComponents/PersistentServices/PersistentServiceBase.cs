using Informagator.Contracts;
using Informagator.Contracts.Configuration;
using Informagator.Contracts.PersistentServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.PersistentServices
{
    public class PersistentServiceBase<TConfig> : IPersistentService<TConfig>
        where TConfig : IPersistentServiceConfiguration
    {
        public TConfig Configuration
        {
            set { throw new NotImplementedException(); }
        }

        public IPersistentServiceSignature Signature
        {
            get { throw new NotImplementedException(); }
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Suspend()
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }
    }
}
