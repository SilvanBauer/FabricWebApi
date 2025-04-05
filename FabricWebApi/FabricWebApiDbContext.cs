using Microsoft.EntityFrameworkCore;

namespace FabricWebApi;

public class FabricWebApiDbContext : DbContext
{
    public FabricWebApiDbContext(DbContextOptions<FabricWebApiDbContext> options)
        : base(options) { }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(au =>
        {
            au.HasKey(au => au.Id);
        });
    }
}
