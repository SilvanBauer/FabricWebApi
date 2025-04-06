using System.Diagnostics;
using System.Text.RegularExpressions;

namespace FabricWebApi.Services;

public class FabricService : IFabricService
{
    private readonly IConfiguration _configuration;

    private readonly Regex _spaceRegex = new Regex(@"\s", RegexOptions.Compiled);

    private static List<FabricRequestLog> _recentRequests = [];

    public FabricService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string AskFabric(string username, string request, string? pattern, string? session)
    {
        // Add to request list containing the last 50 reqeusts
        _recentRequests.Add(new FabricRequestLog(username, request));
        if (_recentRequests.Count > 50)
        {
            _recentRequests = _recentRequests.TakeLast(50).ToList();
        }

        // Construct additional arguments for calling fabricrequest.sh
        var fabricCall = $"\"{request}\"";
        if (!string.IsNullOrWhiteSpace(session))
        {
            // When no pattern is specified "sessiononly" is used as a placeholder
            fabricCall += $" \"{(!string.IsNullOrWhiteSpace(pattern) ? pattern : "sessiononly")}\" \"{_spaceRegex.Replace(username, "_")}-{session}\"";
        }
        else if (!string.IsNullOrWhiteSpace(pattern))
        {
            fabricCall += $" \"{pattern}\"";
        }

        return CallFabric(fabricCall);
    }

    public string GetSessions()
        => CallFabric("--listsessions");

    public void WipeSession(string session)
        => CallFabric($"--wipesession={session}");

    private string CallFabric(string request)
    {
        // Prepare to start process
        var isLinux = _configuration.GetSection("IsLinux").Get<bool>();
        var command = _configuration.GetSection(isLinux ? "CommandLin" : "CommandWin").Get<string>();
        var processStartInfo = new ProcessStartInfo
        {
            FileName = isLinux ? "/bin/bash" : "powershell.exe",
            Arguments = $"{command} {request}",
            WorkingDirectory = Environment.CurrentDirectory,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        // Start process and get output
        var output = new List<string>();
        Action<object, DataReceivedEventArgs> dataReceived = (sender, e) =>
        {
            // Add text to output when a new line is received that isn't empty
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                output.Add(e.Data);
            }
        };
        var process = new Process { StartInfo = processStartInfo };
        process.OutputDataReceived += new DataReceivedEventHandler(dataReceived);
        process.ErrorDataReceived += new DataReceivedEventHandler(dataReceived);
        process.Start();
        process.BeginErrorReadLine();
        process.BeginOutputReadLine();
        process.WaitForExit();

        // Join output list to a single string
        return string.Join('\n', output);
    }

    public string GetRecentRequests()
    {
        // Convert recent requests to string and join them to a single string
        var recentRequests = _recentRequests.Select(r => $"{r.CreatedAt:dd.MM.yyyy - hh:mm:ss} | {r.CreatedBy} | {r.Request}");
        return string.Join('\n', recentRequests);
    }
}
