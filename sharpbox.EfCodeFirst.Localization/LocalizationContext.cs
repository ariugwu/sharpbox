using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using sharpbox.Localization.Model;

namespace sharpbox.EfCodeFirst.Localization
{
  public class LocalizationContext : DbContext
  {

    public LocalizationContext(string name = null)
      : base(string.Format("name= {0}", !string.IsNullOrEmpty(name) ? name : "LocalizationContext"))
    {

    }

    public LocalizationContext()
      : base()
    {

    }

    public DbSet<Resource> Resources { get; set; }
    public DbSet<ResourceName> ResourceNames { get; set; }
    public DbSet<ResourceType> ResourceTypes { get; set; }
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Resource>().HasKey(p => p.ResourceId);
      modelBuilder.Entity<Resource>().Property(x => x.ResourceId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
      modelBuilder.Entity<Resource>().ToTable("Resource", schemaName: "Localization");

      modelBuilder.Entity<ResourceName>().HasKey(x => x.ResourceNameId);
      modelBuilder.Entity<ResourceName>().Property(x => x.ResourceNameId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
      modelBuilder.Entity<ResourceName>().ToTable("ResourceNames", schemaName: "Localization");

      modelBuilder.Entity<ResourceType>().HasKey(x => x.ResourceTypeId);
      modelBuilder.Entity<ResourceType>().Property(x => x.ResourceTypeId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
      modelBuilder.Entity<ResourceType>().ToTable("ResourceTypes", schemaName: "Localization");

    }
  }
}
