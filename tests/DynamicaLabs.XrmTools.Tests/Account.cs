using DynamicaLabs.XrmTools.Construction.Attributes;

namespace DynamicaLabs.XrmTools.Tests
{
    [CrmEntity("account")]
    public class Account
    {
        [CrmField("name")]
        public string Name { get; set; }
    }
}