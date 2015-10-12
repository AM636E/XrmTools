using System;
using System.Collections.Generic;
using System.Linq;
using DynamicaLabs.XrmTools.Construction;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Xunit;
using Assert = Xunit.Assert;

namespace DynamicaLabs.XrmTools.Tests
{
    public class ReflectionEntityConstructorTests
    {
        [Fact]
        public void CreateColumnSet()
        {
            var constructor = new ReflectionEntityConstructor();
            var columnSet = constructor.CreateColumnSet<TestModel>();

            var expectedColumnSet = new ColumnSet("crmid", "name", "surname");
            foreach (var column in expectedColumnSet.Columns)
            {
                Assert.Equal(true, columnSet.Columns.Contains(column));
            }
        }

        [Fact]
        public void CreateObject()
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
            var obj = constructor.ConstructObject<TestModel>(entity);
            Assert.Equal(entity.GetAttributeValue<Guid>("crmid"), obj.Id);
            Assert.Equal(entity.GetAttributeValue<string>("name"), obj.Name);
            Assert.Equal(entity.GetAttributeValue<string>("surname"), obj.Surname);
            Assert.Equal(entity.GetAttributeValue<EntityReference>("entityreference").Id, obj.EntityReferenceField);
            Assert.Equal(entity.GetAttributeValue<OptionSetValue>("optionset").Value, obj.OptionSetField);
        }

        [Fact]
        public void CreateObjectEntityReferenceNull()
        {
            var entity = new Entity
            {
                Attributes =
                {
                    new KeyValuePair<string, object>("entityreference", null)
                }
            };
            var constructor = new ReflectionEntityConstructor();
            var obj = constructor.ConstructObject<TestModel>(entity);
            Assert.Equal(Guid.Empty, obj.EntityReferenceField);
        }

        [Fact]
        public void CreateObjectOptionSetNull()
        {
            var entity = new Entity
            {
                Attributes =
                {
                    new KeyValuePair<string, object>("optionset", null)
                }
            };
            var constructor = new ReflectionEntityConstructor();
            var obj = constructor.ConstructObject<TestModel>(entity);
            Assert.Equal(0, obj.OptionSetField);
        }

        [Fact]
        public void CreateAttributeCollection()
        {
            var obj = new TestModel
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

        [Fact]
        public void TestWithRequiresEntity()
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
            var test = new ReflectionEntityConstructor().ConstructObject<TestModel>(entity);

            Assert.NotNull(test.VerySpecificProp);
            foreach (var val in entity.Attributes.Select(a => a.Value))
            {
                Assert.Contains(val, test.VerySpecificProp);
            }
        }

       
    }
    // ReSharper disable once ClassNeverInstantiated.Local
}