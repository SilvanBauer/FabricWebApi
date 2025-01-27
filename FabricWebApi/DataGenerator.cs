using Microsoft.EntityFrameworkCore;

namespace FabricWebApi
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new FabricWebApiDbContext(serviceProvider.GetRequiredService<DbContextOptions<FabricWebApiDbContext>>());
            if (context.ApplicationUsers.Any())
            {
                return;
            }

            context.ApplicationUsers.Add(new ApplicationUser
            {
                Id = 1,
                Username = "SilvanBauer",
                Password = "WSSQJVXMCicaLKm0Dy4EAM2Wangvqxi1C03ll4VfEyI=" // Hashed with SHA-256
            });
            context.SaveChanges();
        }
    }
}
