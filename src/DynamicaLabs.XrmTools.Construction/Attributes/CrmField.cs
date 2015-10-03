using System;

namespace DynamicaLabs.XrmTools.Construction.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CrmField : Attribute
    {
        public string CrmName { get; set; }
        public Type FieldHandler { get; set; }
        public bool RequiresEntity { get; set; }

        public CrmField() { }

        public CrmField(string crmName)
        {
            CrmName = crmName;
        }
    }
}