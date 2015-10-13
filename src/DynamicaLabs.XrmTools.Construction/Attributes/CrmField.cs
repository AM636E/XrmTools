using System;

namespace DynamicaLabs.XrmTools.Construction.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CrmField : Attribute
    {
        public CrmField()
        {
        }

        public CrmField(string crmName)
        {
            CrmName = crmName;
        }

        public string CrmName { get; set; }
        public Type FieldHandler { get; set; }
        public bool RequiresEntity { get; set; }
    }
}