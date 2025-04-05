namespace FabricWebApi.Services;

public interface IBlockedUsersService
{
    bool IsUserBlocked(string username);

    void ChangeUserState(string username, bool block);
}
