namespace JobManager.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ConfigModel : DbContext
    {
        public ConfigModel()
            : base("name=SQLJobManager")
        {
        }

        public virtual DbSet<ActivityLog> ActivityLogs { get; set; }
        public virtual DbSet<EditableCategory> EditableCategories { get; set; }
        public virtual DbSet<ServerConfig> ServerConfigs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
