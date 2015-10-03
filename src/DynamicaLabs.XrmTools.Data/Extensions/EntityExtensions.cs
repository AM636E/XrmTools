using System.Linq;
using DynamicaLabs.XrmTools.Core;
using Microsoft.Xrm.Sdk;

namespace DynamicaLabs.XrmTools.Data.Extensions
{
    public static class EntityExtensions
    {
        public static RequestAttributeCollection ToRequestAttributeCollection(this Entity entity)
        {
            var result = new RequestAttributeCollection();
            result.AddRange(entity.Attributes.Select(attribute => new RequestAttribute(attribute.Key, attribute.Value)));

            return result;
        }
    }
}