using Microsoft.Xrm.Sdk.Query;

namespace DynamicaLabs.XrmTools.Core.Configuration
{
    public interface IConfiguration
    {
        CheckResult CheckRequest(string entityType, string operation, RequestAttributeCollection attributes);
        CheckResult CheckRequest(string entityType, RequestAttributeCollection attributes);
        bool EntityAllowed(string entityType);
        ColumnSet GetColumnSet(string entityType);
        bool Throw { get; set; }
    }
}