namespace DynamicaLabs.XrmTools.Core.Configuration
{
    public interface IConfiguration
    {
        bool Throw { get; set; }
        CheckResult CheckRequest(string entityType, string operation, RequestAttributeCollection attributes);
    }
}