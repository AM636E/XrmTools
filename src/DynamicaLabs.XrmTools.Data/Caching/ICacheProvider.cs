namespace DynamicaLabs.XrmTools.Data.Caching
{
    public interface ICacheProvider
    {
        void Put(string key, object value, bool updateTime = true);
        CacheEntry Get(string key);
        bool IsSet(string key);
        void Invalidate(string key);
        T Get<T>(string key);
    }
}