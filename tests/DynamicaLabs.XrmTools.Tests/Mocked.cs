using Microsoft.Xrm.Sdk;
using Moq;

namespace DynamicaLabs.XrmTools.Tests
{
    public static class Mocked
    {
        public static IOrganizationService MockedService()
        {
            return Mock.Of<IOrganizationService>(it => true);
        }
    }
}