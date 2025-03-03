namespace FabricWebApi.Services
{
    public interface IFabricService
    {
        string CallFabric(string username, string question, string? pattern);

        string GetRecentRequests();
    }
}
