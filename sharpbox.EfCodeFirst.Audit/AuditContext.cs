using System.Data.Entity;
using sharpbox.Dispatch.Model;

namespace sharpbox.EfCodeFirst.Audit
{
    public class AuditContext : DbContext
    {
        public AuditContext(string name = null)
            : base(string.Format("name= {0}", !string.IsNullOrEmpty(name) ? name : "AuditContext"))
        {

        }

        public DbSet<Response> Responses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Response>().ToTable("Response", schemaName: "Audit");
        }
    }
}
