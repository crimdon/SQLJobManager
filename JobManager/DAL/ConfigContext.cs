using JobManager.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace JobManager.DAL
{
    public class ConfigContext : DbContext
    {
        public ConfigContext() : base("ConfigContext")
        {
        }

        public DbSet<ServerConfig> ServerConfiguration { get; set; }
        public DbSet<EditableCategories> EditableCategories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}