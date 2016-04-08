using System.Threading.Tasks;

namespace DynamicaLabs.Tools.Logging
{
    /// <summary>
    /// Represents a logger.
    /// </summary>
    public interface ILogger
    {
        void Log(LogType logType, string message);
        void LogMessage(string message);
        void LogError(string message);
        void LogWarning(string message);
#if !NET40
        Task LogAsync(LogType logType, string message);
      
        Task LogMessageAsync(string message);
       
        Task LogErrorAsync(string message);
        
        Task LogWarningAsync(string message);
#endif
    }
}