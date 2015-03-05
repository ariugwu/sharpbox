using System.ComponentModel.DataAnnotations.Schema;
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
        public DbSet<EventNames> EventNames { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // We want to store the Entity and Type as serialized and the automatically deseralize on the way out. @SEE: http://stackoverflow.com/a/14785553
            modelBuilder.Entity<Response>().HasKey(p => p.ResponseId);
            modelBuilder.Entity<Response>().Property(x => x.ResponseId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Response>().Property(p => p.SerializedEntity).HasColumnName("Entity");
            modelBuilder.Entity<Response>().Ignore(p => p.Entity);
            modelBuilder.Entity<Response>().Property(p => p.SerializeEntityType).HasColumnName("Type");
            modelBuilder.Entity<Response>().Ignore(p => p.Type);
            modelBuilder.Entity<Response>().ToTable("Response", schemaName: "Audit");
            
            modelBuilder.Entity<EventNames>().ToTable("EventName", schemaName: "Audit");
        }
    }
}
