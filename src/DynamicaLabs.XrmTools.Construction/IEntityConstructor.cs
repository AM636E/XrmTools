using DynamicaLabs.XrmTools.Core;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DynamicaLabs.XrmTools.Construction
{
    public interface IEntityConstructor
    {
        TObject ConstructObject<TObject>(Entity entity);
        ColumnSet CreateColumnSet<TObject>();
        RequestAttributeCollection CreateAttributeCollection<TObject>(TObject obj, bool excludeEmpty = false);
    }
}