using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DynamicaLabs.XrmTools.Construction.Attributes;
using DynamicaLabs.XrmTools.Core;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json.Linq;

namespace DynamicaLabs.XrmTools.Construction
{
    public class ReflectionEntityConstructor : IEntityConstructor
    {
        public TObject ConstructObject<TObject>(Entity entity)
        {
            var type = typeof(TObject);
            var properties = GetTypeProperties(type);
            var result = Activator.CreateInstance<TObject>();

            foreach (var pair in properties)
            {
                var prop = pair.Key;
                var attr = pair.Value;
                try
                {
                    // Skip if entity does not contains field.
                    if (!entity.Contains(attr.CrmName)) continue;

                    var value = entity[attr.CrmName];
                    // Execute handler if present.
                    if (attr.FieldHandler != null && value != null)
                    {
                        value = ExecuteFieldHandler(entity, attr, value);
                    }
                    // Call parse method if types does not match.
                    if (value != null && value.GetType() != prop.PropertyType)
                    {
                        if (prop.PropertyType == typeof(EntityReference))
                        {
                            value = HandleEntityReferenceField(value);
                        }
                        else if (prop.PropertyType == typeof(Money))
                        {
                            value = HandleMoneyField(value);
                        }
                        else if (prop.PropertyType == typeof(OptionSetValue))
                        {
                            value = HandleOptionSetField(value);
                        }
                        else
                        {
                            var meth =
                                prop
                                    .PropertyType
                                    .GetMethod("Parse", new[] { typeof(string) });
                            if (meth != null)
                                value = meth.Invoke(null, new object[] { value.ToString() });
                        }
                    }
                    prop.SetValue(result, value, null);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to set value. Property {prop.Name}. Attribute {attr.CrmName}. Message: {ex.Message}");
                }
            }

            return result;
        }

        

        private static object HandleEntityReferenceField(object value)
        {
            var jvalue = value as JArray;
            return jvalue?.ToObject<EntityReference[]>().First();
        }

        private static object HandleMoneyField(object value)
        {
            return new Money(decimal.Parse(value.ToString()));
        }

        private static object HandleOptionSetField(object value)
        {
            return new OptionSetValue(int.Parse(value.ToString()));
        }

        public ColumnSet CreateColumnSet<TObject>()
        {
            var result = new ColumnSet();

            result.AddColumns(
                typeof(TObject).GetProperties()
                    .Select(p => p.GetCustomAttributes(typeof(CrmField), false).FirstOrDefault())
                    .Where(a => a != null)
                    .Select(p => (CrmField)p)
                    .Select(p => p.CrmName)
                    .ToArray());

            return result;
        }

        public RequestAttributeCollection CreateAttributeCollection<TObject>(TObject obj, bool excludeEmpty = false)
        {
            var result = new RequestAttributeCollection();
            var properties = GetTypeProperties(typeof(TObject));
            foreach (var pair in properties)
            {
                var prop = pair.Key;
                var attr = pair.Value;
                if (obj == null) continue;
                var val = prop.GetValue(obj, null);
                if (excludeEmpty && Utils.IsEmpty(val)) continue;
                result.Add(new RequestAttribute(attr.CrmName, val));
            }
            return result;
        }

        public static Dictionary<PropertyInfo, CrmField> GetTypeProperties(Type type)
        {
            return type
                .GetProperties()
                .ToDictionary(p => p, p => p.GetCustomAttributes(typeof(CrmField), false).FirstOrDefault())
                // Attribute of type CrmField exists on property.
                .Where(kv => kv.Value != null)
                .ToDictionary(k => k.Key, v => (CrmField)v.Value);
        }

        public static object ExecuteFieldHandler(Entity entity, CrmField attr, object value)
        {
            var handlerInst = Activator.CreateInstance(attr.FieldHandler);
            object[] arguments;
            MethodInfo method;
            if (attr.RequiresEntity)
            {
                method =
                    attr
                        .FieldHandler
                        .GetMethod("HandleField", new[] { value.GetType(), typeof(Entity) });
                arguments = new[] { value, entity };
            }
            else
            {
                method =
                    attr
                        .FieldHandler
                        .GetMethod("HandleField", new[] { value.GetType() });
                arguments = new[] { value };
            }

            var cvalue = value;

            try
            {
                value = method.Invoke(handlerInst, arguments);
            }
            catch (Exception)
            {
                value = cvalue;
            }
            return value;
        }
    }
}