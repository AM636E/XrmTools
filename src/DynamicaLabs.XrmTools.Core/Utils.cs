using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using static System.String;

namespace DynamicaLabs.XrmTools.Core
{
    public static class Utils
    {
        public static bool IsEmpty(object value)
        {
            if (value == null)
                return true;
            var s = value as string;
            if (s != null)
                return IsNullOrEmpty(value.ToString());
            if (value is Guid)
                return (Guid) value == Guid.Empty;
            if (value is DateTime)
                return (DateTime) value == DateTime.MinValue;
            var setValue = value as OptionSetValue;
            if (setValue != null)
                return setValue.Value == 0;
            if (value is int)
                return (int) value == 0;
            if (value is double)
                return Math.Abs((double) value) == 0.0;
            return false;
        }

        public static QueryExpression ToQueryExpression(string entityName, IDictionary<string, object> attributes,
            bool strict = true)
        {
            var query = new QueryExpression(entityName)
            {
                Criteria = new FilterExpression(LogicalOperator.And)
            };

            attributes
                .Select(
                    a =>
                        new ConditionExpression(a.Key, strict ? ConditionOperator.Equal : ConditionOperator.Like,
                            a.Value))
                .ToList()
                .ForEach(a => query.Criteria.AddCondition(a));

            return query;
        }
    }
}