namespace FabricWebApi.Services
{
    public interface IFabricService
    {
        string CallFabric(string username, string question);

        string GetRecentRequests();
    }
}
