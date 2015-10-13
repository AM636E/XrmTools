using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using static System.String;
using XrmAttributeCollection = Microsoft.Xrm.Sdk.AttributeCollection;

namespace DynamicaLabs.XrmTools.Core
{
    [JsonConverter(typeof (RequestAttributeConverter))]
    public class RequestAttributeCollection : List<RequestAttribute>
    {
        public string[] Keys
        {
            get { return this.Select(a => a.Key).ToArray(); }
        }

        /// <summary>
        ///     Getter throws exception if key not found.
        ///     Setter removed previous value if exists.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                var attribute = this.FirstOrDefault(a => a.Key == key);
                if (attribute == null)
                {
                    throw new Exception($"attribute '{key}' does not exists");
                }
                return attribute.Value;
            }
            set
            {
                if (this.Any(i => i.Key == key))
                {
                    Remove(this.FirstOrDefault(i => i.Key == key));
                }
                Add(new RequestAttribute(key, value));
            }
        }

        /// <summary>
        ///     Converts this attribute collection to <see cref="XrmAttributeCollection" />
        /// </summary>
        /// <param name="excludeEmpty">Exclude empty values</param>
        /// <returns></returns>
        public XrmAttributeCollection ToEntityAttributeCollection(bool excludeEmpty = false)
        {
            var attrs = new XrmAttributeCollection();

            ForEach(a =>
            {
                // Skipping id attribute because name of id attribute unique for entity(accountid, contactid).
                if (a.Key == "id") return;
                if (excludeEmpty && Utils.IsEmpty(a.Value)) return;
                if (attrs.Any(it => it.Key == a.Key)) return;
                attrs.Add(new KeyValuePair<string, object>(a.Key, a.Value));
            });

            return attrs;
        }

        public new void Add(RequestAttribute attribute)
        {
            if (IsNullOrEmpty(attribute.Key)) return;
            base.Add(attribute);
        }

        public override string ToString()
        {
            return "{\n" + Join(",\n", this.Select(a => $"\"{a.Key}\" : \"{a.Value}\"")) + "\n}";
        }
    }
}