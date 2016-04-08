using System;

namespace DynamicaLabs.XrmTools.Construction.Attributes
{
    /// <summary>
    /// Marks property as ready for construction.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CrmField : Attribute
    {
        public CrmField()
        { }

        public CrmField(string crmName)
        {
            CrmName = crmName;
        }

        public bool Main { get; set; }
        public string CrmName { get; set; }
        public Type FieldHandler { get; set; }
        public bool RequiresEntity { get; set; }
    }
}