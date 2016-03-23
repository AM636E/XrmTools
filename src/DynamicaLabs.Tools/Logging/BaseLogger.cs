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
    }
}