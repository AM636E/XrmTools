using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamicaLabs.Tools
{
    /// <summary>
    /// Represents a settings provider.
    /// </summary>
    public interface ISettingsProvider
    {
        /// <summary>
        /// Get many settings from many keys.
        /// </summary>
        /// <param name="keys">Keys of settings</param>
        /// <returns>Settings</returns>
        IEnumerable<KeyValuePair<string, string>> GetMany(string[] keys);
        Task<IEnumerable<KeyValuePair<string, string>>> GetManyAsync(string[] keys);
        string Get(string key);
        Task<string> GetAsync(string key);

        void SetMany(IEnumerable<KeyValuePair<string, string>> values);
        void SetManyAsync(IEnumerable<KeyValuePair<string, string>> values);
        void Set(KeyValuePair<string, string> value);
        void SetAsync(KeyValuePair<string, string> value);

        string[] CheckRequired(string[] required);
    }
}