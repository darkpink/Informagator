using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts.PersistentServices
{
    public interface IPersistentServiceClient
    {
        IPersistentServiceSignature[] RequiredPersistentServices { get; }
    }
}
