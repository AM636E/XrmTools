using System;
using System.Collections;
using DynamicaLabs.XrmTools.Construction;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DynamicaLabs.XrmTools.Testing
{
    public static class TestUtils
    {
        public static IOrganizationService OrganizationService { get; set; }

        public static void AssertEqual<TObject>(TObject obj, Entity entity, bool strict = false)
        {
            var props = ReflectionEntityConstructor.GetTypeProperties(typeof(TObject));
            foreach (var prop in props)
            {
                var c = entity.Contains(prop.Value.CrmName);
                if (!c && strict)
                    throw new AssertException($"Assertion failed. Field {prop.Value.CrmName} doesn't present on entity.");
                if (!c) continue;
                var modelVal = prop.Key.GetValue(obj, null);
                var entityVal = entity[prop.Value.CrmName];
                if (prop.Value.FieldHandler != null && entityVal != null)
                {
                    entityVal = ReflectionEntityConstructor.ExecuteFieldHandler(entity, prop.Value, entityVal);
                }
                // Skip for specific types.
                if (modelVal is IEnumerable && !(modelVal is string)) continue;
                
                if (!modelVal.Equals(entityVal))
                    throw new AssertException($"Assertion failed. Property {prop.Key.Name} = {modelVal}. Entity {prop.Value.CrmName} = {entityVal}");
            }
        }

        public static void AssertEqual<TObject>(TObject obj, string entityType, Guid entityGuid, bool strict = false)
        {
            AssertEqual(obj, OrganizationService.Retrieve(entityType, entityGuid, new ColumnSet(true)), strict);
        }
    }
}