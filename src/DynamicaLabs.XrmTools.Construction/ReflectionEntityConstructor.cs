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
        public object ConstructObject(Entity entity, Type objectType)
        {
            var type = objectType;
            var properties = GetTypePropertiesMultiple(type);
            var result = Activator.CreateInstance(objectType);

            foreach (var pair in properties)
            {
                var prop = pair.Key;
                var attrs = pair.Value;
                foreach (var attr in attrs)
                {
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
                            value = ParseValue(prop, value);
                        }
                        prop.SetValue(result, value, null);
                    }
                    catch (Exception ex)
                    {
                        throw new ConstructionException(
                            $"Failed to set value. Property {prop.Name}. Attribute {attr.CrmName}. Message: {ex.Message}");
                    }
                }
            }

            var nonProps = type.GetProperties().Where(p => !properties.ContainsKey(p));
            foreach (var prop in nonProps)
            {
                try
                {
                    prop.SetValue(result, ConstructObject(entity, prop.PropertyType), null);
                }
                catch (Exception)
                {
                    // Silent.
                }
            }

            return result;
        }

        private static Dictionary<PropertyInfo, CrmField[]> GetTypePropertiesMultiple(Type type)
        {
            return type
                .GetProperties()
                .ToDictionary(p => p,
                    p => p.GetCustomAttributes(typeof (CrmField), false).Select(a => (CrmField) a).ToArray())
                .Where(p => p.Value.Any())
                .ToDictionary(p => p.Key, p => p.Value);
        }

        public TObject ConstructObject<TObject>(Entity entity)
        {
            return (TObject)ConstructObject(entity, typeof(TObject));
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

        public Entity ConstructEntity<TObject>(TObject obj)
        {
            var eType = typeof(TObject);
            var cattr = eType.GetCustomAttributes(false).FirstOrDefault(a => a.GetType() == typeof(CrmEntity));
            if (cattr == null)
                throw new ArgumentException("Class must be decorated with CrmEntity Attribute to use with CreateEntity");
            var attr = (CrmEntity)cattr;
            if (string.IsNullOrEmpty(attr.EntityName))
                throw new ArgumentException("EntityName of CrmEntity Attribute must not be empty.");
            var crmEntity = new Entity(attr.EntityName)
            {
                Attributes = CreateAttributeCollection(obj).ToEntityAttributeCollection()
            };
            if (crmEntity.Contains($"{attr.EntityName}id"))
            {
                crmEntity.Attributes.Remove(crmEntity.Attributes.FirstOrDefault(a => a.Key == $"{attr.EntityName}id"));
            }
            return crmEntity;
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

        public static Dictionary<PropertyInfo, CrmField> GetTypeProperties(Type type)
        {
            var dic = type
                .GetProperties()
                // Select only main attribute.
                .ToDictionary(p => p, p => p.GetCustomAttributes(typeof(CrmField), false).Select(a => (CrmField)a));
            var result = new Dictionary<PropertyInfo, CrmField>();
            foreach (var item in dic)
            {
                if (item.Value.Count() > 1)
                {
                    result.Add(item.Key, item.Value.First(a => a.Main));
                }
                else if (item.Value.Count() == 1)
                {
                    result.Add(item.Key, item.Value.First());
                }
            }
            return result;
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

        private static object ParseValue(PropertyInfo prop, object value)
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
            return value;
        }
    }
}