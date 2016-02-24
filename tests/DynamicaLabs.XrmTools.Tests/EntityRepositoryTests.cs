﻿using System.Collections.Generic;
using System.Linq;
using DynamicaLabs.XrmTools.Construction;
using DynamicaLabs.XrmTools.Construction.Attributes;
using DynamicaLabs.XrmTools.Data;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using Xunit;

namespace DynamicaLabs.XrmTools.Tests
{
    [Construction.Attributes.CrmEntity("account")]
    public class Account
    {
        [CrmField("name")]
        public string Name { get; set; }
    }

    public static class Mocked
    {
        public static IOrganizationService MockedService()
        {
            return Mock.Of<IOrganizationService>(it => true);
        }
    }

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


    public class EntityRepositoryTests
    {
        private static readonly EntityCollection Ents = new EntityCollection(new List<Entity>
            {
                new Entity("account") { ["name"] = "test1" },
                new Entity("account") { ["name"] = "test2" },
                new Entity("account") { ["name"] = "test3" },
                new Entity("account") { ["name"] = "test4" },
            });

        private readonly DefaultEntityRepository _repo;

        public EntityRepositoryTests()
        {
            var s = new Mock<OrganizationServiceAdapter>();
            s.Setup(it => it.RetrieveMultiple(It.IsAny<QueryBase>())).Returns(Ents);
            _repo = DefaultEntityRepository.FromOrganizationService(s.Object, new ReflectionEntityConstructor());
        }

        [Fact]
        public void TestRetrieve()
        {

            var ents = _repo.GetEntities<Account>(new QueryExpression("account")).ToList();
            Assert.Equal(Ents.Entities.Count, ents.Count);
            for (var i = 1; i <= 4; i++)
            {
                Assert.Equal($"test{i}", ents[i - 1].Name);
            }
        }
    }
}