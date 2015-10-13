using System;
using Microsoft.Xrm.Sdk;

namespace DynamicaLabs.XrmTools.Construction
{
    public class MoneyFieldHandler : IFieldHandler<Money, decimal>
    {
        public decimal HandleField(Money field)
        {
            return field?.Value ?? 0;
        }

        public decimal HandleField(Money field, Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}