using System;
using Microsoft.Xrm.Sdk;

namespace DynamicaLabs.XrmTools.Construction
{
    public class NameEnRefFieldHandler : IFieldHandler<EntityReference, string>
    {
        public string HandleField(EntityReference field)
        {
            return field == null ? string.Empty : field.Name;
        }

        public string HandleField(EntityReference field, Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}