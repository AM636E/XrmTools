using System;
using System.Collections.Generic;

namespace DynamicaLabs.XrmTools.Data.Caching
{
    public class InMemoryCacheProvider : ICacheProvider
    {
        private static readonly Dictionary<string, object> Cache = new Dictionary<string, object>();

        public void Put(string key, object value, bool updateTime = true)
        {
            throw new NotImplementedException();
        }

        public CacheEntry Get(string key)
        {
            throw new NotImplementedException();
        }

        public bool IsSet(string key)
        {
            throw new NotImplementedException();
        }

        public void Invalidate(string key)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }
    }
}