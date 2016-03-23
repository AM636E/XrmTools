using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicaLabs.Tools
{
    public abstract class BaseSettingsProvider : ISettingsProvider
    {
        public abstract IEnumerable<KeyValuePair<string, string>> GetMany(string[] keys);
        public abstract void SetMany(IEnumerable<KeyValuePair<string, string>> values);

#if !NET40
        public virtual async Task<IEnumerable<KeyValuePair<string, string>>> GetManyAsync(string[] keys)
        {
            return await new Task<IEnumerable<KeyValuePair<string, string>>>(() => GetMany(keys));
        }
#else
        public virtual Task<IEnumerable<KeyValuePair<string, string>>> GetManyAsync(string[] keys)
        {
            return null;
        }
#endif

        public string Get(string key)
        {
            var a = GetMany(new[] { key }).ToList();
            return a.Any() ? a.First().Value : string.Empty;
        }


#if !NET40
        public virtual async Task<string> GetAsync(string key)
        {
            var result = new Task<string>(() => Get(key));

            return await result;
        }
#else
        public virtual Task<string> GetAsync(string key)
        {
            return null;
        }
#endif


#if !NET40
        public virtual async void SetManyAsync(IEnumerable<KeyValuePair<string, string>> values)
        {
            await Task.Factory.StartNew(() => SetMany(values));
        }
#else
        public virtual void SetManyAsync(IEnumerable<KeyValuePair<string, string>> values)
        {

        }
#endif


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
        public virtual async void SetAsync(KeyValuePair<string, string> value)
        {
            await Task.Factory.StartNew(() => Set(value));
        }

        
#else
        public virtual void SetAsync(KeyValuePair<string, string> value)
        {
        }
#endif

    }
}