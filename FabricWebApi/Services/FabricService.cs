using System.Diagnostics;

namespace FabricWebApi.Services
{
    public class FabricService : IFabricService
    {
        private readonly IConfiguration _configuration;

        public FabricService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ExecuteCommand()
        {
            var output = new List<string>();
            Action<object, DataReceivedEventArgs> dataReceived = (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.Data))
                {
                    output.Add(e.Data);
                }
            };

            var isLinux = _configuration.GetSection("IsLinux").Get<bool>();
            var command = _configuration.GetSection(isLinux ? "CommandLin" : "CommandWin").Get<string>();
            var processStartInfo = new ProcessStartInfo
            {
                FileName = isLinux ? "/bin/bash" : "powershell.exe",
                Arguments = command,
                WorkingDirectory = Environment.CurrentDirectory,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            var process = new Process { StartInfo = processStartInfo };
            process.OutputDataReceived += new DataReceivedEventHandler(dataReceived);
            process.ErrorDataReceived += new DataReceivedEventHandler(dataReceived);
            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            process.WaitForExit();

            return string.Join('\n', output);
        }
    }
}
