using System;

namespace DynamicaLabs.XrmTools.Construction.Attributes
{
    /// <summary>
    /// Marks entity as crm entity. Required for create from model.
    /// </summary>
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