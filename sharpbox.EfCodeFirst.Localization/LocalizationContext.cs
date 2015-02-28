using System.Data.Entity;
using sharpbox.Localization.Model;

namespace sharpbox.EfCodeFirst.Localization
{
    public class LocalizationContext : DbContext
    {
        public DbSet<Resource> Resources { get; set; }
        public DbSet<ResourceNames> ResourceNames { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resource>().ToTable("Resource", schemaName: "Localization");
            modelBuilder.Entity<ResourceNames>().ToTable("ResourceNames", schemaName: "Localization");
        }
    }
}
