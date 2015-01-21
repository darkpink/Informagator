using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts.Services
{
    public static class InfoServiceAddress
    {
        public static string Format(string hostName, int port)
        {
            return String.Format("http://{0}:{1}/InfoService", hostName, port);
        }
    }
}
