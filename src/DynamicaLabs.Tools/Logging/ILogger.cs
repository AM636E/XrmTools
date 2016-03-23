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
    }
}