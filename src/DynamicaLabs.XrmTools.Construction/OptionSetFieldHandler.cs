using Microsoft.Xrm.Sdk;

namespace DynamicaLabs.XrmTools.Construction
{
    public class OptionSetFieldHandler : IFieldHandler<OptionSetValue, int>
    {
        public int HandleField(OptionSetValue field)
        {
            return field == null ? 0 : field.Value;
        }

        public int HandleField(OptionSetValue field, Entity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}