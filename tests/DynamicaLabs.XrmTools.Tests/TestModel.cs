using System;
using DynamicaLabs.XrmTools.Construction;
using DynamicaLabs.XrmTools.Construction.Attributes;

namespace DynamicaLabs.XrmTools.Tests
{
    [CrmEntity("testentity")]
    public class TestModel
    {
        [CrmField("testentityid")]
        public Guid CrmId { get; set; }
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

        [CrmField(CrmName = "optionset", FieldHandler = typeof(TestFieldHandler), RequiresEntity = true)]
        // ReSharper disable once UnusedMember.Local
        public object[] VerySpecificProp { get; set; }

        // ReSharper disable once UnusedMember.Local
        public int NotSettedProp { get; set; }

        public TestInnerClass Item { get; set; }
        public TestClass TestClass { get; set; }

        public class TestInnerClass
        {
            [CrmField(CrmName = "crmid")]
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public Guid Id { get; set; }

            [CrmField(CrmName = "name")]
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Name { get; set; }
        }
    }

    public class TestClass
    {
        [CrmField(CrmName = "entityreference", FieldHandler = typeof(GuidEntityReferenceFieldHandler))]
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public Guid EntityReferenceField { get; set; }

        [CrmField(CrmName = "optionset", FieldHandler = typeof(OptionSetFieldHandler))]
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public int OptionSetField { get; set; }

        [CrmField(CrmName = "optionset", FieldHandler = typeof(TestFieldHandler), RequiresEntity = true)]
        // ReSharper disable once UnusedMember.Local
        public object[] VerySpecificProp { get; set; }
    }
}