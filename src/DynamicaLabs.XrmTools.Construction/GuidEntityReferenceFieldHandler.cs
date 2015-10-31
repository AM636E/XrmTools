using System;
using Microsoft.Xrm.Sdk;

namespace DynamicaLabs.XrmTools.Construction
{
    /// <summary>
    /// Retrieves guid from EntityReference.
    /// Guid.Empty if null.
    /// </summary>
    public class GuidEntityReferenceFieldHandler : IFieldHandler<EntityReference, Guid>
    {
        public Guid HandleField(EntityReference field)
        {
            return field?.Id ?? Guid.Empty;
        }

        public Guid HandleField(EntityReference field, Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}