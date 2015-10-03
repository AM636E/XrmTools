using System;
using System.Collections.Generic;
using DynamicaLabs.XrmServiceToolkit.Construction;
using DynamicaLabs.XrmServiceToolkit.Construction.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Xunit;
using Assert = Xunit.Assert;

namespace DynamicaLabs.Project.Tests
{
    [TestClass]
    public class ReflectionEntityConstructorTests
    {
        [TestMethod]
        public void ReflectionEntityConstructor_CreateColumnSet()
        {
            var constructor = new ReflectionEntityConstructor();
            var columnSet = constructor.CreateColumnSet<Test>();

            var expectedColumnSet = new ColumnSet("crmid", "name", "surname");
            foreach (var column in expectedColumnSet.Columns)
            {
                Assert.Equal(true, columnSet.Columns.Contains(column));
            }
        }

        [Fact]
        public void ReflectionEntityConstructor_CreateObject()
        {
            var entity = new Entity
            {
                Attributes =
                {
                    new KeyValuePair<string, object>("crmid", Guid.Parse("FD93433F-694E-4B30-9B7E-668B3E53A8E5")),
                    new KeyValuePair<string, object>("name", "Gordon"),
                    new KeyValuePair<string, object>("surname", "Shumway"),
                    new KeyValuePair<string, object>("entityreference", new EntityReference("", Guid.Parse("FD93433F-694E-4B30-9B7E-668B3E53A8E5"))),
                    new KeyValuePair<string, object>("optionset", new OptionSetValue(100)),
                }
            };
            var constructor = new ReflectionEntityConstructor();
            var obj = constructor.ConstructObject<Test>(entity);
            Assert.Equal(entity.GetAttributeValue<Guid>("crmid"), obj.Id);
            Assert.Equal(entity.GetAttributeValue<string>("name"), obj.Name);
            Assert.Equal(entity.GetAttributeValue<string>("surname"), obj.Surname);
            Assert.Equal(entity.GetAttributeValue<EntityReference>("entityreference").Id, obj.EntityReferenceField);
            Assert.Equal(entity.GetAttributeValue<OptionSetValue>("optionset").Value, obj.OptionSetField);
        }

        [Fact]
        public void ReflectionEntityConstructor_CreateObjectEntityReferenceNull()
        {
            var entity = new Entity
            {
                Attributes =
                {
                    new KeyValuePair<string, object>("entityreference", null)
                }
            };
            var constructor = new ReflectionEntityConstructor();
            var obj = constructor.ConstructObject<Test>(entity);
            Assert.Equal(Guid.Empty, obj.EntityReferenceField);
        }

        [Fact]
        public void ReflectionEntityConstructor_CreateObjectOptionSetNull()
        {
            var entity = new Entity
            {
                Attributes =
                {
                    new KeyValuePair<string, object>("optionset", null)
                }
            };
            var constructor = new ReflectionEntityConstructor();
            var obj = constructor.ConstructObject<Test>(entity);
            Assert.Equal(0, obj.OptionSetField);
        }

        [Fact]
        public void ReflectionEntityConstructor_CreateAttributeCollection()
        {
            var obj = new Test
            {
                Id = Guid.Parse("FD93433F-694E-4B30-9B7E-668B3E53A8E5"),
                Surname = "Shumway",
                Name = "Gordon"
            };
            var constructor = new ReflectionEntityConstructor();
            var attributes = constructor.CreateAttributeCollection(obj);
            Assert.Equal(obj.Id, attributes["crmid"]);
            Assert.Equal(obj.Surname, attributes["surname"]);
            Assert.Equal(obj.Name, attributes["name"]);
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class Test
        {
            [CrmField(CrmName = "crmid")]
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public Guid Id { get; set; }

            [CrmField(CrmName = "name")]
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Name { get; set; }

            [CrmField(CrmName = "surname")]
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Surname { get; set; }

            [CrmField(CrmName = "entityreference", FieldHandler = typeof(GuidEntityReferenceFieldHandler))]
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public Guid EntityReferenceField { get; set; }

            [CrmField(CrmName = "optionset", FieldHandler = typeof(OptionSetFieldHandler))]
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public int OptionSetField { get; set; }

            // ReSharper disable once UnusedMember.Local
            public int NotSettedProp { get; set; }
        }
    }
}