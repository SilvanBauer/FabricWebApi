using Microsoft.EntityFrameworkCore;

namespace FabricWebApi;

public class DataGenerator
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        if (!configuration.GetSection("UseInMemory").Get<bool>())
        {
            return;
        }

        using var context = new FabricWebApiDbContext(serviceProvider.GetRequiredService<DbContextOptions<FabricWebApiDbContext>>());
        var staticUsers = configuration.GetSection("StaticUsers").Get<StaticUser[]>();
        if (staticUsers != null)
        {
            foreach (var staticUser in staticUsers)
            {
                if (context.ApplicationUsers.Any(au => au.Username == staticUser.Username))
                {
                    continue;
                }

                context.ApplicationUsers.Add(new ApplicationUser
                {
                    Username = staticUser.Username,
                    Password = staticUser.Password // Hashed with SHA-256
                });
            }

            context.SaveChanges();
        }
    }
}
