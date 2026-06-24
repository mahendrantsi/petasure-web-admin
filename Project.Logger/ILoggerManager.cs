using System;

namespace Project.Logger
{
    public interface ILoggerManager
    {
        void LogException(Exception exception);
        void LogInformation(string information);
        void LogTrace(string trace);
        void LogWarning(string warning);
        void LogDebug(string debug);
    }
}
