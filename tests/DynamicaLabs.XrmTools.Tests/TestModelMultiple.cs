using System;
using DynamicaLabs.XrmTools.Construction.Attributes;

namespace DynamicaLabs.XrmTools.Tests
{
    public class TestModelMultiple
    {
        [CrmField("productid", Main = true), CrmField("dnl_productid")]
        public Guid Id { get; set; }
        [CrmField("productname", Main = true), CrmField("dnl_productname")]
        public string Name { get; set; } 
    }
}