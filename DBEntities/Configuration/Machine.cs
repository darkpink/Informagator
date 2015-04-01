//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Informagator.DBEntities.Configuration
{
    using System;
    using System.Collections.Generic;
    
    public partial class Machine
    {
        public Machine()
        {
            this.Workers = new HashSet<Worker>();
            this.MachineErrorHandlers = new HashSet<MachineErrorHandler>();
        }
    
        public long Id { get; set; }
        public long SystemConfigurationId { get; set; }
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public string Description { get; set; }
        public int AdminServicePort { get; set; }
        public int InfoServicePort { get; set; }
        public bool SuppressSystemConfigurationErrorHandlers { get; set; }
    
        public virtual SystemConfiguration SystemConfiguration { get; set; }
        public virtual ICollection<Worker> Workers { get; set; }
        public virtual ICollection<MachineErrorHandler> MachineErrorHandlers { get; set; }
    }
}
