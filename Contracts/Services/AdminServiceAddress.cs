using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts.Services
{
    public static class AdminServiceAddress
    {
        public static string Format(string hostName, int port)
        {
            return String.Format("http://{0}:{1}/AdminService", hostName, port);
        }
    }
}
