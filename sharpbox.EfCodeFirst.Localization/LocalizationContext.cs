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
      : base("name=LocalizationContext")
    {

    }

    public DbSet<Resource> Resources { get; set; }
    public DbSet<ResourceName> ResourceNames { get; set; }
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Resource>().HasKey(p => p.ResourceId);
      modelBuilder.Entity<Resource>().Property(x => x.ResourceId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
      modelBuilder.Entity<Resource>().ToTable("Resource", schemaName: "Localization");

      modelBuilder.Entity<ResourceName>().HasKey(x => x.ResourceNameId);
      modelBuilder.Entity<ResourceName>().Property(x => x.ResourceNameId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
      modelBuilder.Entity<ResourceName>().ToTable("ResourceNames", schemaName: "Localization");

      modelBuilder.Entity<EmailTemplate>().HasKey(x => x.EmailTemplateId);
      modelBuilder.Entity<EmailTemplate>().Property(x => x.EmailTemplateId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
      modelBuilder.Entity<EmailTemplate>().ToTable("EmailTemplate", schemaName: "Localization");

    }
  }
}
