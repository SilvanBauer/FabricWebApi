using Microsoft.EntityFrameworkCore;

namespace FabricWebApi
{
    public class FabricWebApiDbContext : DbContext
    {
        public FabricWebApiDbContext(DbContextOptions<FabricWebApiDbContext> options)
            : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
