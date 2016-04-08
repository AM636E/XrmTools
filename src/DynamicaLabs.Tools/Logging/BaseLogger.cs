#if !NET40
using System;
using System.Threading.Tasks;
#endif

namespace DynamicaLabs.Tools.Logging
{
    public abstract class BaseLogger : ILogger
    {
        public abstract void Log(LogType logType, string message);
        
        public void LogMessage(string message)
        {
            Log(LogType.Message, message);
        }

        public void LogError(string message)
        {
            Log(LogType.Error, message);
        }

        public void LogWarning(string message)
        {
            Log(LogType.Warning, message);
        }

#if !NET40
        public virtual async Task LogAsync(LogType logType, string message)
        {
            await Task.Run(() => Log(logType, message));
        }
        public virtual async Task LogMessageAsync(string message)
        {
            await LogAsync(LogType.Message, message);
        }
        public virtual async Task LogErrorAsync(string message)
        {
            await LogAsync(LogType.Message, message);
        }
        public virtual async Task LogWarningAsync(string message)
        {
            await LogAsync(LogType.Message, message);
        }
#endif
    }
}