using DynamicaLabs.XrmTools.Core;

namespace DynamicaLabs.XrmTools.Data
{
    public interface IRequestVerifier
    {
        bool VerifyRequest(string entityType, string operation, RequestAttributeCollection request, out string message);
    }
}