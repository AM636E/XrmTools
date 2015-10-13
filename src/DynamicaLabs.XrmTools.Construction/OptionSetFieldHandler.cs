using System;
using Microsoft.Xrm.Sdk;

namespace DynamicaLabs.XrmTools.Construction
{
    public class OptionSetFieldHandler : IFieldHandler<OptionSetValue, int>
    {
        public int HandleField(OptionSetValue field)
        {
            return field?.Value ?? 0;
        }

        public int HandleField(OptionSetValue field, Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}