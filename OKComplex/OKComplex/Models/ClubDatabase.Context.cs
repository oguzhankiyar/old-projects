﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OKComplex.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class InfosecEntities : DbContext
    {
        public InfosecEntities()
            : base("name=InfosecEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<club_category> club_category { get; set; }
        public DbSet<club_config> club_config { get; set; }
        public DbSet<club_groupmember> club_groupmember { get; set; }
        public DbSet<club_member> club_member { get; set; }
        public DbSet<club_menulink> club_menulink { get; set; }
        public DbSet<club_notice> club_notice { get; set; }
        public DbSet<club_page> club_page { get; set; }
        public DbSet<club_post> club_post { get; set; }
        public DbSet<club_postcomment> club_postcomment { get; set; }
        public DbSet<club_group> club_group { get; set; }
    }
}
