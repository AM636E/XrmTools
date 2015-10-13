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

        /// <summary>
        /// Check if annotated object properties equals to entity properties.
        /// </summary>
        /// <typeparam name="TObject">Type of object being compared</typeparam>
        /// <param name="obj">Object to check.</param>
        /// <param name="entity">Entity to check with</param>
        /// <param name="strict">If true checks if entity contains all fields that in object.</param>
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

        /// <summary>
        /// Retrieves entity from Crm and executes <see cref="AssertEqual{TObject}(TObject,Microsoft.Xrm.Sdk.Entity,bool)"/>
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="obj"></param>
        /// <param name="entityType"></param>
        /// <param name="entityGuid"></param>
        /// <param name="strict"></param>
        public static void AssertEqual<TObject>(TObject obj, string entityType, Guid entityGuid, bool strict = false)
        {
            AssertEqual(obj, OrganizationService.Retrieve(entityType, entityGuid, new ColumnSet(true)), strict);
        }
    }
}