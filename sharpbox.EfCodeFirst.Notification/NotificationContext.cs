using System.Data.Entity;
using sharpbox.Notification.Model;

namespace sharpbox.EfCodeFirst.Notification
{
    /// <summary>
    /// @SEE : http://devproconnections.com/entity-framework/working-schema-names-entity-framework-code-first-design
    /// </summary>
    public class NotificationContext : DbContext
    {
        public DbSet<BackLogItem> BackLogItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BackLogItem>().ToTable("BackLogItem", schemaName: "Notification");
        }
    }
}
