﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KOK.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class KOKEntities : DbContext
    {
        public KOKEntities()
            : base("name=KOKEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<resim> resim { get; set; }
        public DbSet<sayfa> sayfa { get; set; }
        public DbSet<yonetici> yonetici { get; set; }
        public DbSet<link> link { get; set; }
        public DbSet<duyuru> duyuru { get; set; }
        public DbSet<basvuru> basvuru { get; set; }
    }
}
