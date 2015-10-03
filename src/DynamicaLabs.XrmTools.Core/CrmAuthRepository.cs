using System;
using System.Net;
using System.Security.Principal;
using System.ServiceModel.Description;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk.Client;

namespace DynamicaLabs.XrmTools.Core
{
    public class CrmAuthRepository : ICrmAuthRepository
    {
        private readonly string _uri;

        public CrmAuthRepository(string uri)
        {
            _uri = uri;
        }

        public SystemUser FindUser(string userName, string password)
        {
            var credentials = new ClientCredentials();
            credentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
            credentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
            credentials.UserName.UserName = userName;
            credentials.UserName.Password = password;
            using (var proxy = new OrganizationServiceProxy(new Uri(_uri + "/XRMServices/2011/Organization.svc"), null,
                credentials, null))
            {
                var whoAmiResponse = proxy.Execute<WhoAmIResponse>(new WhoAmIRequest());

                return new SystemUser
                {
                    Id = whoAmiResponse.UserId.ToString(),
                    UserName = userName,
                    Password = password
                };
            }
        }
    }
}