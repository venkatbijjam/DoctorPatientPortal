﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DP_Portal
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DP_PortalEntities : DbContext
    {
        public DP_PortalEntities()
            : base("name=DP_PortalEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<USER> USERS { get; set; }
        public virtual DbSet<USERS_TYPE> USERS_TYPE { get; set; }
        public virtual DbSet<APPOINTMENT> APPOINTMENTS { get; set; }
        public virtual DbSet<APPOINTMENT_DETAILS> APPOINTMENT_DETAILS { get; set; }
        public virtual DbSet<PATIENT_PRESCRPTIONS> PATIENT_PRESCRPTIONS { get; set; }
        public virtual DbSet<PATIENT_DETAILS> PATIENT_DETAILS { get; set; }
        public virtual DbSet<SLOT> SLOTS { get; set; }
        public virtual DbSet<ORGAN_DONORS> ORGAN_DONORS { get; set; }
        public virtual DbSet<DOCTOR_SPEC> DOCTOR_SPEC { get; set; }
    }
}
