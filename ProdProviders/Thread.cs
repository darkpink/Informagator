//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Acadian.Informagator.ProdProviders
{
    using System;
    using System.Collections.Generic;
    
    public partial class Thread
    {
        public Thread()
        {
            this.Stages = new HashSet<Stage>();
        }
    
        public string Name { get; set; }
        public string HostName { get; set; }
        public bool IsEnabled { get; set; }
        public string WorkerAssembly { get; set; }
        public string WorkerType { get; set; }
    
        public virtual Host Host { get; set; }
        public virtual ICollection<Stage> Stages { get; set; }
    }
}
