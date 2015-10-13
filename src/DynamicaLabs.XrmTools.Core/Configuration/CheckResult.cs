namespace DynamicaLabs.XrmTools.Core.Configuration
{
    public class CheckResult
    {
        public enum CheckStatus
        {
            Valid,
            Invalid
        }

        public CheckResult(CheckStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public CheckStatus Status { get; set; }
        public string Message { get; set; }
    }
}