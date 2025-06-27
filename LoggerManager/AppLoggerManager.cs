using Contracts;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerManager;

public class AppLoggerManager : ILoggerManager
{
    private static ILogger _logger = LogManager.GetCurrentClassLogger();
    public AppLoggerManager()
    {
        
    }

    public void LogWarning(string message)
    {
        _logger.Warn(message);
    }

    public void LogConsole(string message)
    {
        Console.WriteLine(message);
    }

    public void LogDebug(string message)
    {
        _logger.Debug(message);
    }

    public void LogError(string message)
    {
        _logger.Error(message);
    }

    public void LogInfo(string message)
    {
        _logger.Info(message);
    }
}
