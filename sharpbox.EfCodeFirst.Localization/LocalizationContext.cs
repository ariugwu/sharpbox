using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using sharpbox.Localization.Model;

namespace sharpbox.EfCodeFirst.Localization
{
    public class LocalizationContext : DbContext
    {

        public LocalizationContext(string name = null) : base(string.Format("name= {0}", !string.IsNullOrEmpty(name)? name : "LocalizationContext"))
        {
            
        }

        public DbSet<Resource> Resources { get; set; }
        public DbSet<ResourceNames> ResourceNames { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resource>().HasKey(p => p.ResourceId);
            modelBuilder.Entity<Resource>().Property(x => x.ResourceId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ResourceNames>().HasKey(x => x.ResourceNamesId);
            modelBuilder.Entity<ResourceNames>().Property(x => x.ResourceNamesId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Resource>().ToTable("Resource", schemaName: "Localization");
            modelBuilder.Entity<ResourceNames>().ToTable("ResourceNames", schemaName: "Localization");
        }
    }
}
