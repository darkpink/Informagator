﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ConfigurationEntities : DbContext
    {
        public ConfigurationEntities()
            : base("name=ConfigurationEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AssemblySystemConfiguration> AssemblySystemConfigurations { get; set; }
        public virtual DbSet<AssemblyVersion> AssemblyVersions { get; set; }
        public virtual DbSet<ErrorHandlerParameter> ErrorHandlerParameters { get; set; }
        public virtual DbSet<GlobalSetting> GlobalSettings { get; set; }
        public virtual DbSet<Machine> Machines { get; set; }
        public virtual DbSet<Stage> Stages { get; set; }
        public virtual DbSet<StageParameter> StageParameters { get; set; }
        public virtual DbSet<SystemConfiguration> SystemConfigurations { get; set; }
        public virtual DbSet<Worker> Workers { get; set; }
    }
}
