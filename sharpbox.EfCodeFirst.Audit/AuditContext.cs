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
            modelBuilder.Entity<Response>().ToTable("Response", schemaName: "Audit");
            modelBuilder.Entity<EventNames>().ToTable("EventName", schemaName: "Audit");
            modelBuilder.Entity<Response>().Property(x => x.RequestId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
