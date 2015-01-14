using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.PersistentServices
{
    public class SocketClientPersistentService : PersistentServiceBase<SocketClientPersistentServiceConfiguration>
    {
        protected SocketClientPersistentServiceConfiguration Configuration { get; set; }
        public IPersistentServiceSignature Signature
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public void Suspend()
        {
        }

        public void Resume()
        {
        }
    }
}
