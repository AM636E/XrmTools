using System.Collections.Generic;
using System.Linq;
using DynamicaLabs.Tools.Encryption;

namespace DynamicaLabs.Tools
{
    public sealed class EncryptedSettingsProvider : BaseSettingsProvider
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly IEnctyptionService _enctyptionService;
        private readonly IEnumerable<string> _encryptedKeys;

        public EncryptedSettingsProvider(ISettingsProvider settingsProvider, IEnctyptionService enctyptionService, IEnumerable<string> encryptedKeys)
        {
            _settingsProvider = settingsProvider;
            _enctyptionService = enctyptionService;
            _encryptedKeys = encryptedKeys;
        }

        public override IEnumerable<KeyValuePair<string, string>> GetMany(string[] keys)
        {
            var data = _settingsProvider.GetMany(keys);

            return
                data.Select(
                    it =>
                        _encryptedKeys.Contains(it.Key)
                            ? new KeyValuePair<string, string>(it.Key, _enctyptionService.Decrypt(it.Value))
                            : it);
        }

        public override void SetMany(IEnumerable<KeyValuePair<string, string>> values)
        {
            var svalues = values.Select(
                it =>
                    _encryptedKeys.Contains(it.Key)
                        ? new KeyValuePair<string, string>(it.Key, _enctyptionService.Encrypt(it.Value))
                        : it);

            _settingsProvider.SetMany(svalues);
        }
    }
}