using Microsoft.Xrm.Sdk.Query;

namespace DynamicaLabs.XrmTools.Core.Configuration
{
    public interface IConfiguration
    {
        CheckResult CheckRequest(string entityType, string operation, RequestAttributeCollection attributes);
        bool Throw { get; set; }
    }
}