namespace ComponentTradeCenter.Server.Services
{
    public class BackgroundDumpService : BackgroundService
    {
        public static readonly string DumpDirectory = "Dumps";
        public static readonly TimeSpan DumpInterval = TimeSpan.FromMinutes(5);
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BackgroundDumpService> _logger;

        public BackgroundDumpService(IServiceProvider serviceProvider, ILogger<BackgroundDumpService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("BackgroundDumpService started");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DumpDb();
                    await ClearDumps();
                    await Task.Delay(DumpInterval, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occured in BackgroundDumpService");
                }
            }
            _logger.LogInformation("BackgroundDumpService exited");
        }

        protected async Task DumpDb()
        {
            var timestamp = DateTime.Now;
            var dumpService = _serviceProvider.GetRequiredService<DumpDbService>();
            var outputFile = $"Dumps/dump_{timestamp:yyyyMMdd_HHmmss}.sql";
            await dumpService.DumpAsync("localhost", "root", "123456", "ComponentTradeCenter", outputFile);
            _logger.LogInformation("Database dumped to {OutputFile}", outputFile);
        }

        protected async Task ClearDumps()
        {
            var dumpService = _serviceProvider.GetRequiredService<ClearDumpService>();
            await dumpService.ClearOutdateDumps(DumpDirectory);
        }
    }
}
