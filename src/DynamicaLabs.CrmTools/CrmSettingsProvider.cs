using System.Collections.Generic;
using System.Linq;
using DynamicaLabs.Tools;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DynamicaLabs.CrmTools
{
    /// <summary>
    /// Implements settings providers where settings are stored in crm entities.
    /// </summary>
    public sealed class CrmSettingsProvider : BaseSettingsProvider
    {
        private readonly IOrganizationService _organizationService;
        private readonly string _entityName;
        private readonly string _keyField;
        private readonly string _valueField;
        private readonly string _connectionString;

        /// <summary>
        /// Initializes new instance of CrmSettingsProvider.
        /// </summary>
        /// <param name="entityName">Setting entity name.</param>
        /// <param name="keyField">Key field name.</param>
        /// <param name="valueField">Value field name.</param>
        /// <param name="connectionString"></param>
        public CrmSettingsProvider(string entityName, string keyField, string valueField, string connectionString)
        {
            _entityName = entityName.ToLower();
            _keyField = keyField.ToLower();
            _valueField = valueField.ToLower();
            _connectionString = connectionString;
        }

        /// <summary>
        /// Initializes new instance of CrmSettingsProvider.
        /// </summary>
        /// <param name="entityName">Setting entity name.</param>
        /// <param name="keyField">Key field name.</param>
        /// <param name="valueField">Value field name.</param>
        /// <param name="organizationService">Organization service to use.</param>
        public CrmSettingsProvider(string entityName, string keyField, string valueField, IOrganizationService organizationService) :
            this(entityName, keyField, valueField, string.Empty)
        {
            _organizationService = organizationService;
        }

        private IOrganizationService GetOrganizationService()
        {
            return _organizationService;
        }

        private IEnumerable<Entity> GetManyEntities(string[] keys)
        {
            var service = GetOrganizationService();
            var query = new QueryExpression(_entityName)
            {
                ColumnSet = new ColumnSet(_keyField, _valueField),
                Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression("statecode", ConditionOperator.Equal, 0),
                            new ConditionExpression(_keyField, ConditionOperator.NotNull)
                        }
                    }
            };
            var filter = new FilterExpression(LogicalOperator.Or);
            keys.Select(it => new ConditionExpression(_keyField, ConditionOperator.Equal, it)).ToList()
                .ForEach(filter.AddCondition);
            query.Criteria.AddFilter(filter);
            var ents = service.RetrieveMultiple(query).Entities;
            return ents;
        }

        public override IEnumerable<KeyValuePair<string, string>> GetMany(string[] keys)
        {
            return GetManyEntities(keys)
                .Select(it =>
                    new KeyValuePair<string, string>(
                        it.GetAttributeValue<string>(_keyField),
                        it.GetAttributeValue<string>(_valueField)));
        }
        
        public override void SetMany(IEnumerable<KeyValuePair<string, string>> values)
        {
            var keyValuePairs = values as KeyValuePair<string, string>[] ?? values.ToArray();
            var entities = GetManyEntities(keyValuePairs.Select(it => it.Key).ToArray()).ToList();
            // Keys that exists in crm.
            var eKeys = entities.Select(it => it.GetAttributeValue<string>(_keyField));
            // Pairs that exists in crm.
            var ePairs = keyValuePairs.Where(it => eKeys.Any(ek => ek == it.Key));
            // Pairs that don't exists in crm. They will be created.
            var nePairs = keyValuePairs.Where(it => !eKeys.Contains(it.Key)).ToList();

            // Select keys to update.
            var toUpdate = ePairs.Select(it => new Entity(_entityName)
            {
                Id = entities.First(e => e.GetAttributeValue<string>(_keyField) == it.Key).Id,
                [_valueField] = it.Value
            });
            var service = GetOrganizationService();
            foreach (var nEntity in toUpdate)
            {
                service.Update(nEntity);
            }

            var toCreate = nePairs.Select(it => new Entity(_entityName)
            {
                [_keyField] = it.Key,
                [_valueField] = it.Value
            });

            foreach (var entity in toCreate)
            {
                service.Create(entity);
            }
        }
    }
}