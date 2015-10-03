using System;
using Microsoft.Xrm.Sdk;

namespace DynamicaLabs.XrmTools.Construction
{
    public class GuidEntityReferenceFieldHandler : IFieldHandler<EntityReference, Guid>
    {
        public Guid HandleField(EntityReference field)
        {
            return field == null ? Guid.Empty : field.Id;
        }

        public Guid HandleField(EntityReference field, Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}