using System;
using System.Linq;
using DynamicaLabs.XrmTools.Construction;
using Microsoft.Xrm.Sdk;

namespace DynamicaLabs.XrmTools.Tests
{
    public class TestFieldHandler : IFieldHandler<object, object[]>
    {
        public object[] HandleField(object fieldValue)
        {
            throw new NotImplementedException();
        }

        public object[] HandleField(object field, Entity entity)
        {
            return entity.Attributes.Select(a => a.Value).ToArray();
        }
    }
}