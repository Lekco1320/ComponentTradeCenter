namespace ComponentTradeCenter.Server.Services
{
    public class ClearDumpService
    {
        public static readonly TimeSpan RetentionDuration = TimeSpan.FromMinutes(30);
        private readonly ILogger<ClearDumpService> _logger;

        public ClearDumpService(ILogger<ClearDumpService> logger)
        {
            _logger = logger;
        }

        public Task ClearOutdateDumps(string directory)
        {
            var directoryInfo = new DirectoryInfo(directory);
            foreach (var file in directoryInfo.GetFiles("*.sql"))
            {
                if (DateTime.Now - file.LastWriteTime > RetentionDuration)
                {
                    try
                    {
                        file.Delete();
                        _logger.LogInformation("Deleted outdate dump file {file}", file);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error occured in clearing outdated dump file");
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
