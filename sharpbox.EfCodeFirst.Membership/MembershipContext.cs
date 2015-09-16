using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using sharpbox.Membership.Model;

namespace sharpbox.EfCodeFirst.Membership
{
  public class MembershipContext : DbContext
  {
    public MembershipContext(string name = null)
      : base(string.Format("name= {0}", !string.IsNullOrEmpty(name) ? name : "MembershipContext"))
    {

    }

    public MembershipContext()
      : base()
    {

    }
    public DbSet<UserRoleName> UserRoleNames { get; set; }
    public DbSet<UserUserRole> UserUserRoles { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Entity<UserRoleName>().HasKey(p => p.UserRoleNameId);
      modelBuilder.Entity<UserRoleName>().Property(x => x.UserRoleNameId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
      modelBuilder.Entity<UserRoleName>().ToTable("UserRoleName", schemaName: "Membership");

      modelBuilder.Entity<UserUserRole>().HasKey(p => p.UserUserRoleId);
      modelBuilder.Entity<UserUserRole>().Property(x => x.UserUserRoleId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
      modelBuilder.Entity<UserUserRole>().ToTable("UserUserRole", schemaName: "Membership");

    }
  }
}
