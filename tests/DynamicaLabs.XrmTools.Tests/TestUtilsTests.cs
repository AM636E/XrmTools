﻿using System;
using System.Collections.Generic;
using DynamicaLabs.XrmTools.Construction;
using DynamicaLabs.XrmTools.Testing;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Xunit;

namespace DynamicaLabs.XrmTools.Tests
{
    public class TestUtilsTests
    {
        private Entity CreatEntity()
        {
            return new Entity
            {
                Attributes =
                {
                    new KeyValuePair<string, object>("crmid", Guid.Parse("FD93433F-694E-4B30-9B7E-668B3E53A8E5")),
                    new KeyValuePair<string, object>("name", "Gordon"),
                    new KeyValuePair<string, object>("surname", "Shumway"),
                    new KeyValuePair<string, object>("entityreference",
                        new EntityReference("", Guid.Parse("FD93433F-694E-4B30-9B7E-668B3E53A8E5"))),
                    new KeyValuePair<string, object>("optionset", new OptionSetValue(100))
                }
            };
        }
        [Fact]
        public void TestNoException()
        {
            var entity = CreatEntity();
            var constructor = new ReflectionEntityConstructor();
            var obj = constructor.ConstructObject<TestModel>(entity);
            TestUtils.AssertEqual(obj, entity);
        }

        [Fact]
        public void TestWithException()
        {
            var entity = CreatEntity();
            var constructor = new ReflectionEntityConstructor();
            var obj = constructor.ConstructObject<TestModel>(entity);
            obj.Name = "exg;lksrg";
            Exception ex = Assert.Throws<AssertException>(() => { TestUtils.AssertEqual(obj, entity); });

            Assert.Equal(
                $"Assertion failed. Property Name = {obj.Name}. Entity name = {entity["name"]}",
                ex.Message
                );
        }

        [Fact]
        public void TestWithExcluded()
        {
            var entity = CreatEntity();
            var obj = new ReflectionEntityConstructor().ConstructObject<TestModel>(entity);
            obj.Name = "nadia";
            TestUtils.AssertEqual(obj, entity, new[] { "name" });
        }
    }
}