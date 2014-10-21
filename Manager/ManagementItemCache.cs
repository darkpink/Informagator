using Acadian.Informagator.ProdProviders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Net;
using System.Collections;

namespace Acadian.Informagator.Manager
{
    public class ManagementItemCache
    {
        public IEnumerable Machines { get; set; }

        public IEnumerable Threads { get; set; }

        public IEnumerable Assemblies { get; set; }

        public IEnumerable Versions { get; set; }

        public void LoadVersion(long applicationVersionId)
        {
            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                Machines = entities.Hosts
                           .Include(h => h.Threads)
                           .Where(h => h.ApplicationVersion.Id == applicationVersionId);

                Assemblies = entities.AssemblyApplicationVersions
                                 .Where(aav => aav.ApplicationVersionId == applicationVersionId)
                                 .Select(aav => aav.AssemblyVersion);

                Threads = entities.Threads
                              .Where(t => t.Host.ApplicationVersionId == applicationVersionId);
                              
                
                Versions = entities.ApplicationVersions;
            }
        }
    }
}
