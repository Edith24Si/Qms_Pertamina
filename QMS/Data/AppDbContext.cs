using Microsoft.EntityFrameworkCore;
using QMS.Core.Models;

namespace QMS.Core.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TbUpload> TbUploads { get; set; }
        public DbSet<TbMenu> TbMenus { get; set; }
        public DbSet<TbEmployee> TbEmployees { get; set; }
        public DbSet<TbGugus> TbGuguses { get; set; }
        public DbSet<TbUser> TbUsers { get; set; }
        public DbSet<TbRegisteredUser> RegisteredUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
