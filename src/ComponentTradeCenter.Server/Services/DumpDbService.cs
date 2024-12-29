using System.Diagnostics;

namespace ComponentTradeCenter.Server.Services
{
    public class DumpDbService
    {
        private readonly ILogger<DumpDbService> _logger;

        public DumpDbService(ILogger<DumpDbService> logger)
        {
            _logger = logger;
        }

        public async Task DumpAsync(string host, string user, string password, string database, string outputFile)
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c mysqldump -h {host} -u {user} -p{password} {database} > \"{outputFile}\"",
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = new Process
                {
                    StartInfo = processStartInfo
                };

                process.Start();
                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                {
                    var error = await process.StandardError.ReadToEndAsync();
                    throw new Exception($"Database dump failed: {error}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured in dumping database");
            }
        }
    }
}
