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
    
    public partial class StageErrorHandler
    {
        public long Id { get; set; }
        public long StageId { get; set; }
        public long ErrorHandlerId { get; set; }
    
        public virtual ErrorHandler ErrorHandler { get; set; }
        public virtual Stage Stage { get; set; }
    }
}
