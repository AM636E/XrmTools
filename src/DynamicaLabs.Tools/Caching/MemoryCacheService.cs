using System.Runtime.Caching;

namespace DynamicaLabs.Tools.Caching
{
    public sealed class MemoryCacheService : ICasheService
    {
        private static readonly MemoryCache MemCache = new MemoryCache(typeof(MemoryCacheService).Name + "Cache");
        public TObject Get<TObject>(string key)
        {
            if (MemCache.Contains(key))
                return (TObject)MemCache[key];
            return default(TObject);
        }

        public void Set<TObject>(string key, TObject data)
        {
            MemCache[key] = data;
        }
    }
}