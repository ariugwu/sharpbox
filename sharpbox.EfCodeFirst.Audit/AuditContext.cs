﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using sharpbox.Dispatch.Model;

namespace sharpbox.EfCodeFirst.Audit
{
    public class AuditContext : DbContext
    {
        public AuditContext(string name)
            : base(string.Format("name= {0}", !string.IsNullOrEmpty(name) ? name : "AuditContext"))
        {

        }

        public AuditContext()
            : base()
        {

        }

        public DbSet<Response> Responses { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<EventName> EventNames { get; set; }
        public DbSet<CommandName> CommandNames { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // We want to store the Entity and Type as serialized and the automatically deseralize on the way out. @SEE: http://stackoverflow.com/a/14785553
            modelBuilder.Entity<Request>().HasKey(p => p.RequestId);
            modelBuilder.Entity<Request>().Property(x => x.RequestId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Request>().Property(p => p.SerializedEntity).HasColumnName("Entity");
            modelBuilder.Entity<Request>().Ignore(p => p.Entity);
            modelBuilder.Entity<Request>().Property(p => p.SerializeEntityType).HasColumnName("Type");
            modelBuilder.Entity<Request>().Ignore(p => p.Type);
            modelBuilder.Entity<Request>().Ignore(p => p.Action);
            modelBuilder.Entity<Request>().ToTable("Request", schemaName: "Audit");

            modelBuilder.Entity<Response>().HasKey(p => p.ResponseId);
            modelBuilder.Entity<Response>().Property(x => x.ResponseId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Response>().Property(p => p.SerializedEntity).HasColumnName("Entity");
            modelBuilder.Entity<Response>().Ignore(p => p.Entity);
            modelBuilder.Entity<Response>().Property(p => p.SerializeEntityType).HasColumnName("Type");
            modelBuilder.Entity<Response>().Ignore(p => p.Type);
            modelBuilder.Entity<Response>().ToTable("Response", schemaName: "Audit");

            modelBuilder.Entity<EventName>().HasKey(p => p.EventNameId);
            modelBuilder.Entity<EventName>().Property(x => x.EventNameId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<EventName>().ToTable("EventName", schemaName: "App");

            modelBuilder.Entity<CommandName>().HasKey(p => p.CommandNameId);
            modelBuilder.Entity<CommandName>().Property(x => x.CommandNameId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<CommandName>().ToTable("CommandName", schemaName: "App");
        }
    }
}
