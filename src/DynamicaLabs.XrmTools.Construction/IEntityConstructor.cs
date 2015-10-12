using System;
using DynamicaLabs.XrmTools.Core;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DynamicaLabs.XrmTools.Construction
{
    public interface IEntityConstructor
    {
        TObject ConstructObject<TObject>(Entity entity);
        object ConstructObject(Entity entity, Type objectType);
        ColumnSet CreateColumnSet<TObject>();
        RequestAttributeCollection CreateAttributeCollection<TObject>(TObject obj, bool excludeEmpty = false);
        Entity ConstructEntity<TObject>(TObject obj);
    }
}