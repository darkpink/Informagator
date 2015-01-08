//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Informagator.ProdProviders.Configuration
{
    using System;
    using System.Collections.Generic;
    
    public partial class Worker
    {
        public Worker()
        {
            this.Stages = new HashSet<Stage>();
        }
    
        public long Id { get; set; }
        public long MachineId { get; set; }
        public string Name { get; set; }
        public long WorkerAssemblyVersionId { get; set; }
        public string WorkerType { get; set; }
        public bool AutoStart { get; set; }
    
        public virtual AssemblyVersion WorkerAssemblyVersion { get; set; }
        public virtual Machine Machine { get; set; }
        public virtual ICollection<Stage> Stages { get; set; }
    }
}
