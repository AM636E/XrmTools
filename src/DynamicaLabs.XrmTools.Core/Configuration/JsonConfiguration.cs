using System.Collections.Generic;
using System.IO;
using System.Linq;
using DynamicaLabs.XrmTools.Core.Configuration.Exceptions;
using Newtonsoft.Json.Linq;
using static System.String;

namespace DynamicaLabs.XrmTools.Core.Configuration
{
    public class JsonConfiguration : IConfiguration
    {
        private readonly JObject _jConfig;

        public JsonConfiguration(JObject jConfig)
        {
            _jConfig = jConfig;
        }

        public JsonConfiguration(string path)
        {
            _jConfig = JObject.Parse(File.ReadAllText(path));
        }

        public bool Throw { get; set; }

        public CheckResult CheckRequest(string entityType, string operation, RequestAttributeCollection attributes)
        {
            var message = Empty;
            // If operation is not allowed for this entity.
            if (_jConfig[entityType] == null || _jConfig[entityType][operation] == null)
            {
                message = $"Operation {operation} is now alowed for entity {entityType}";
                if (Throw) throw new InvalidRequestException(message);
                return new CheckResult(CheckResult.CheckStatus.Invalid, message);
            }
            // Convert config entry to dictionary of required and optional fields.
            var fields = (_jConfig[entityType][operation]).ToObject<Dictionary<string, JArray>>();

            // Processing required section//Check if request contains required fields.
            var keys = attributes.Keys;
            var allFields = fields["required"].ToObject<string[]>();
            var diff = allFields.Where(f => !keys.Contains(f)).ToList();
            
            // Missing fields
            if (diff.Any())
            {
                // Generate mesage with missing keys.
                message = $"Fields [{Join(", ", diff)}] are missing";
                if (Throw) throw new InvalidRequestException(message);
                return new CheckResult(CheckResult.CheckStatus.Invalid, message);
            }

            // Processing required-or section.
            // Check if request fields have required or fields.
            if (fields.ContainsKey("required-or"))
            {
                var orFields = fields["required-or"].ToObject<string[][]>();
                foreach (var orField in orFields)
                {
                    if (!orField.Intersect(keys).Any())
                    {
                        message = $"Fields [{Join(", ", orField)}] are required";
                        if (Throw) throw new InvalidRequestException(message);
                        return new CheckResult(CheckResult.CheckStatus.Invalid, message);
                    }

                    allFields = allFields.Concat(orField).ToArray();
                }
            }

            // Processing optional section.
            if (fields.ContainsKey("optional"))
            {
                // Add optional fields to all fields.
                allFields = allFields.Concat(fields["optional"].ToObject<string[]>()).ToArray();
            }

            // Check if request does not contain extra fields.
            var extra = keys.Where(k => !allFields.Contains(k)).ToArray();
            // ReSharper disable once InvertIf
            if (extra.Any())
            {
                message = $"Fields [{Join(", ", extra)}] is not allowed";
                if (Throw) throw new InvalidRequestException(message);
                return new CheckResult(CheckResult.CheckStatus.Valid, message);
            }

            return new CheckResult(CheckResult.CheckStatus.Valid, message);
        }
    }
}