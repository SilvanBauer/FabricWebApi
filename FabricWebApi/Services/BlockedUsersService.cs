using System.Collections.Concurrent;

namespace FabricWebApi.Services;

public class BlockedUsersService : IBlockedUsersService
{
    private static ConcurrentDictionary<string, bool> _userStates;

    private readonly FabricWebApiDbContext _dbContext;

    public BlockedUsersService(FabricWebApiDbContext dbContext)
    {
        _userStates ??= new ConcurrentDictionary<string, bool>();
        _dbContext = dbContext;
    }

    public bool IsUserBlocked(string username)
    {
        if (_userStates.TryGetValue(username, out var isBlocked))
        {
            return isBlocked;
        }

        var user = _dbContext.ApplicationUsers.SingleOrDefault(au => au.Username == username);
        if (user == null)
        {
            return false;
        }

        _userStates.TryAdd(user.Username, user.IsBlocked);

        return user.IsBlocked;
    }

    public void ChangeUserState(string username, bool block)
    {
        var user = _dbContext.ApplicationUsers.SingleOrDefault(au => au.Username == username);
        if (user == null)
        {
            return;
        }

        _userStates.AddOrUpdate(username, block, (_, _) => block);
        user.IsBlocked = block;
        _dbContext.SaveChanges();
    }
}
