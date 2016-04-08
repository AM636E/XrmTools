using System;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;

namespace DynamicaLabs.CrmTools
{
    /// <summary>
    /// Organization service factory that returns service by connection.
    /// </summary>
    public class ConnectionOsFactory : IOrganizationServiceFactory
    {
        private readonly CrmConnection _connection;
        public ConnectionOsFactory(CrmConnection connection)
        {
            _connection = connection;
        }

        public IOrganizationService CreateOrganizationService(Guid? userId)
        {
            return new OrganizationService(_connection);
        }
    }
}