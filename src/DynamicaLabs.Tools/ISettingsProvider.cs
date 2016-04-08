using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamicaLabs.Tools
{
    /// <summary>
    /// Represents a simgle key-value settings provider.
    /// </summary>
    public interface ISettingsProvider
    {
        /// <summary>
        /// Get many settings from many keys.
        /// </summary>
        /// <param name="keys">Keys of settings</param>
        /// <returns>Settings</returns>
        IEnumerable<KeyValuePair<string, string>> GetMany(string[] keys);
        
        string Get(string key);
        

        void SetMany(IEnumerable<KeyValuePair<string, string>> values);
       
        void Set(KeyValuePair<string, string> value);

#if !NET40
        Task<string> GetAsync(string key);
        Task<IEnumerable<KeyValuePair<string, string>>> GetManyAsync(string[] keys);
        Task SetManyAsync(IEnumerable<KeyValuePair<string, string>> values);
        Task SetAsync(KeyValuePair<string, string> value);
#endif
    }
}