using DS.Shared.Logging;
using Serilog;

namespace DS.Users.Infrastructure.Logging
{
    public class LoggerManager : ILoggerManager
    {
        private readonly Serilog.ILogger _logger;

        public LoggerManager()
        {
            _logger = new LoggerConfiguration()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public void LogDebug(string message) => _logger.Debug(message);
        public void LogError(string message) => _logger.Error(message);
        public void LogInfo(string message) => _logger.Information(message);
        public void LogWarn(string message) => _logger.Warning(message);
    }
}
