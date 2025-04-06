namespace FabricWebApi.Services;

public interface IFabricService
{
    string AskFabric(string username, string question, string? pattern, string? session);

    string GetSessions();

    void WipeSession(string session);

    string GetRecentRequests();
}
