using System.Collections.Generic;
using System.Linq;
#if !NET40
using System.Threading.Tasks;
#endif

namespace DynamicaLabs.Tools
{
    public abstract class BaseSettingsProvider : ISettingsProvider
    {
        public abstract IEnumerable<KeyValuePair<string, string>> GetMany(string[] keys);
        public abstract void SetMany(IEnumerable<KeyValuePair<string, string>> values);

        public string Get(string key)
        {
            var a = GetMany(new[] { key }).ToList();
            return a.Any() ? a.First().Value : string.Empty;
        }

        public void Set(KeyValuePair<string, string> value)
        {
            SetMany(new[] { value });
        }

        public virtual string[] CheckRequired(string[] required)
        {
            return GetMany(required)
                .Where(it => string.IsNullOrEmpty(it.Value))
                .Select(it => it.Key)
                .ToArray();

        }

#if !NET40
        public virtual async Task<IEnumerable<KeyValuePair<string, string>>> GetManyAsync(string[] keys)
        {
            return await Task.Run(() => GetMany(keys));
        }
        public virtual async Task<string> GetAsync(string key)
        {
            return await Task.Run(() => Get(key));
        }
        public virtual async Task SetManyAsync(IEnumerable<KeyValuePair<string, string>> values)
        {
            await Task.Factory.StartNew(() => SetMany(values));
        }
        public virtual async Task SetAsync(KeyValuePair<string, string> value)
        {
            await Task.Factory.StartNew(() => Set(value));
        }
#endif
    }
}