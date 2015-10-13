using System;
using DynamicaLabs.XrmTools.Core;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DynamicaLabs.XrmTools.Construction
{
    public class JsonEntityConstructor : IEntityConstructor
    {
        public JsonEntityConstructor(string mappingPath)
        {
            
        }

        public TObject ConstructObject<TObject>(Entity entity)
        {
            throw new System.NotImplementedException();
        }

        public object ConstructObject(Entity entity, Type objectType)
        {
            throw new NotImplementedException();
        }

        public TObject ConstructObject<TObject>(Entity entity, Type objectType)
        {
            throw new NotImplementedException();
        }

        public ColumnSet CreateColumnSet<TObject>()
        {
            throw new System.NotImplementedException();
        }

        public RequestAttributeCollection CreateAttributeCollection<TObject>(TObject obj, bool excludeEmpty = false)
        {
            throw new System.NotImplementedException();
        }

        public Entity ConstructEntity<TObject>(TObject obj)
        {
            throw new NotImplementedException();
        }
    }
}