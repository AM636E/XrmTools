using Microsoft.Xrm.Sdk;

namespace DynamicaLabs.XrmTools.Construction
{
    public class MoneyFieldHandler : IFieldHandler<Money, decimal>
    {
        public decimal HandleField(Money field)
        {
            return field == null ? 0 : field.Value;
        }

        public decimal HandleField(Money field, Entity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}