using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DynamicaLabs.Tools
{
    /// <summary>
    /// Represents simple key-value json settings.
    /// </summary>
    public class JsonSettingsProvider : BaseSettingsProvider
    {
        private readonly IFileProvider _fileProvider;
        private readonly string _fileName;

        public JsonSettingsProvider(IFileProvider fileProvider, string fileName)
        {
            _fileProvider = fileProvider;
            _fileName = fileName;
        }

        public override IEnumerable<KeyValuePair<string, string>> GetMany(string[] keys)
        {
            var sval = _fileProvider.GetContentString(_fileName);
            var jobj = JsonConvert.DeserializeObject<JObject>(sval);

            return keys.Select(it => new KeyValuePair<string, string>(it, jobj[it]?.ToString()));
        }

        public override void SetMany(IEnumerable<KeyValuePair<string, string>> values)
        {
            var sval = _fileProvider.GetContentString(_fileName);
            var jobj = JsonConvert.DeserializeObject<JObject>(sval);
            foreach (var pair in values)
            {
                jobj[pair.Key] = pair.Value;
            }

            _fileProvider.SetContent(_fileName, JsonConvert.SerializeObject(jobj, Formatting.Indented));
        }
    }
}