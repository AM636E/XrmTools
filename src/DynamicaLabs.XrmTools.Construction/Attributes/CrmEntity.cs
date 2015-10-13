using System;

namespace DynamicaLabs.XrmTools.Construction.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CrmEntity : Attribute
    {
        public CrmEntity(string entityName)
        {
            EntityName = entityName;
        }

        public string EntityName { get; set; }
    }
}