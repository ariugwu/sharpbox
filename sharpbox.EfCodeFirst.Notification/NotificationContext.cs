using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using sharpbox.Dispatch.Model;
using sharpbox.Notification.Model;

namespace sharpbox.EfCodeFirst.Notification
{
    /// <summary>
    /// @SEE : http://devproconnections.com/entity-framework/working-schema-names-entity-framework-code-first-design
    /// </summary>
    public class NotificationContext : DbContext
    {
        /// <summary>
        /// The Notification context. Will create a table for BackLogItems in the Notification schema if it doesn't already exist
        /// </summary>
        /// <param name="name">The name of the connection string to use. Will use "NotificationContext if none supplied.</param>
        public NotificationContext(string name = null) : base(string.Format("name= {0}", !string.IsNullOrEmpty(name)? name : "NotificationContext"))
        {
            
        }

        public NotificationContext()
            : base()
        {

        }

        public DbSet<BackLogItem> BackLogItems { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<EventNames> EventNames { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BackLogItem>().HasKey(p => p.BackLogItemId);
            modelBuilder.Entity<BackLogItem>().Property(x => x.BackLogItemId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<BackLogItem>().ToTable("BackLogItem", schemaName: "Notification");

            modelBuilder.Entity<Subscriber>().HasKey(p => p.SubscriberId);
            modelBuilder.Entity<Subscriber>().Property(x => x.SubscriberId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Subscriber>().ToTable("Subscriber", schemaName: "Notification");

            modelBuilder.Entity<EventNames>().HasKey(p => p.EventNameId);
            modelBuilder.Entity<EventNames>().Property(x => x.EventNameId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<EventNames>().ToTable("EventName", schemaName: "App");
        }
    }
}
