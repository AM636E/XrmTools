using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace DynamicaLabs.XrmTools.Data
{
    /// <summary>
    /// Retrieves option set from crm metadata.
    /// </summary>
    public class MetadataOptionSetRetriever : IOptionSetRetriever
    {
        private static readonly Dictionary<string, Dictionary<int, string>> ValuesDictionary =
            new Dictionary<string, Dictionary<int, string>>();

        private readonly DefaultEntityRepository _defaultEntityRepository;

        public MetadataOptionSetRetriever(DefaultEntityRepository defaultEntityRepository)
        {
            _defaultEntityRepository = defaultEntityRepository;
        }

        public string GetOptionSetText(string optionSetName, OptionSetValue value, string etn, int def = 0)
        {
            if (string.IsNullOrEmpty(optionSetName))
                throw new ArgumentNullException(nameof(optionSetName));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (value.Value == def)
            {
                return string.Empty;
            }
            var key = etn + optionSetName;
            if (!ValuesDictionary.ContainsKey(key) || !ValuesDictionary[key].ContainsKey(value.Value))
            {
                ValuesDictionary[key] = RetrieveOptionSet(etn, optionSetName);
            }

            return !ValuesDictionary[key].ContainsKey(value.Value) ? string.Empty : ValuesDictionary[key][value.Value];
        }

        public OptionSetValue GetOptionSetValue(string optionSetName, string optionSetString, string entityLogicalName)
        {
            if (string.IsNullOrEmpty(optionSetName))
                throw new ArgumentNullException(nameof(optionSetName));
            if (string.IsNullOrEmpty(optionSetString))
                throw new ArgumentNullException(nameof(optionSetString));
            if (string.IsNullOrEmpty(entityLogicalName))
                throw new ArgumentNullException(nameof(entityLogicalName));
            var key = entityLogicalName + optionSetName;
            if (!ValuesDictionary.ContainsKey(key))
            {
                ValuesDictionary[key] = RetrieveOptionSet(entityLogicalName, optionSetName);
            }
            var optionSet = ValuesDictionary[key];

            if (!optionSet.ContainsValue(optionSetString))
                throw new ArgumentException(
                    $"Option set {optionSetName} does not contains {optionSetString}",
                    nameof(optionSetString));
            return
                new OptionSetValue(
                    optionSet.FirstOrDefault(
                        a => string.Compare(a.Value, optionSetString, StringComparison.InvariantCultureIgnoreCase) == 0)
                        .Key);
        }

        public Dictionary<int, string> GetMapping(string entityName, string optionSetName)
        {
            var key = entityName + optionSetName;
            if (!ValuesDictionary.ContainsKey(key))
            {
                ValuesDictionary[key] = RetrieveOptionSet(entityName, optionSetName);
            }

            return ValuesDictionary[key];
        }

        private Dictionary<int, string> RetrieveOptionSet(string etn, string optionSetName)
        {
            var service = _defaultEntityRepository.GetOrganizationService();
            OptionSetMetadata osMeta;
            try
            {
                var response = (RetrieveOptionSetResponse) service.Execute(new RetrieveOptionSetRequest
                {
                    Name = optionSetName
                });
                osMeta = (OptionSetMetadata) response.OptionSetMetadata;
            }
            catch (Exception)
            {
                var response = (RetrieveEntityResponse) service.Execute(new RetrieveEntityRequest
                {
                    EntityFilters = EntityFilters.All,
                    LogicalName = etn
                });

                osMeta =
                    ((PicklistAttributeMetadata)
                        response.EntityMetadata.Attributes.First(
                            a => string.Equals(a.LogicalName, optionSetName, StringComparison.OrdinalIgnoreCase)))
                        .OptionSet;
            }

            return osMeta
                .Options
                .Select(
                    m =>
                        m.Value != null
                            ? new KeyValuePair<int, string>(m.Value.Value, m.Label.UserLocalizedLabel.Label)
                            : new KeyValuePair<int, string>(-1, string.Empty))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}