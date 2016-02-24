using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;

namespace DynamicaLabs.XrmTools.Tests
{
    public class OrganizationServiceAdapter : OrganizationService
    {
        public OrganizationServiceAdapter() : base(Mocked.MockedService())
        {

        }

        public OrganizationServiceAdapter(string connectionStringName) : base(connectionStringName)
        {
        }

        public OrganizationServiceAdapter(CrmConnection connection) : base(connection)
        {
        }

        public OrganizationServiceAdapter(IOrganizationService service) : base(service)
        {
        }
    }
}