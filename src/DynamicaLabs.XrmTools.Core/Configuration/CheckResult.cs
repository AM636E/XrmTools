namespace DynamicaLabs.XrmTools.Core.Configuration
{
    public class CheckResult
    {
        public enum CheckStatus
        {
            Valid, Invalid
        }

        public CheckStatus Status { get; set; }
        public string Message { get; set; }

        public CheckResult(CheckStatus status, string message)
        {
            Status = status;
            Message = message;
        }
    }
}