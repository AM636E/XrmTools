using System;

namespace DynamicaLabs.XrmTools.Construction.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CrmEntity : Attribute
    {
        public string EntityName { get; set; }

        public CrmEntity(string entityName)
        {
            EntityName = entityName;
        }
    }
}