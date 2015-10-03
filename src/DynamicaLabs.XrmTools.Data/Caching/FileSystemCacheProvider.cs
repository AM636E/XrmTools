using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DynamicaLabs.XrmTools.Data.Caching
{
    public class FileSystemCacheProvider : ICacheProvider
    {
        private readonly string _basePath;

        public FileSystemCacheProvider(string basePath)
        {
            _basePath = basePath;
        }

        public void Put(string key, object value, bool updateTime = true)
        {
            var val = Get(key);
            if (val == null && !updateTime)
            {
                throw new Exception("Cache entry is not present, so time must be set.");
            }
            key += ".cache";
            try
            {
                if (!File.Exists(_basePath + key))
                {
                    File.Create(_basePath + key);
                }
                var info = new FileInfo(_basePath + key);
                var entry = val;
                using (var streamwriter = new StreamWriter(info.Open(FileMode.Truncate)))
                {
                    if (entry == null)
                    {
                        entry = new CacheEntry();
                    }
                    if (updateTime)
                    {
                        entry.InsertionDate = DateTime.Now;
                    }

                    entry.Value = value;

                    streamwriter.Write(JsonConvert.SerializeObject(entry));
                }
            }
            catch (Exception)
            {
                // 
            }
        }

        public CacheEntry Get(string key)
        {
            key += ".cache";
            using (var stream = File.OpenRead(_basePath + key))
            using (var reader = new StreamReader(stream))
            {
                return JsonConvert.DeserializeObject<CacheEntry>(reader.ReadToEnd());
            }
        }

        public bool IsSet(string key)
        {
            key += ".cache";
            return File.Exists(_basePath + key);
        }

        public void Invalidate(string key)
        {
            key += ".cache";
            File.Delete(_basePath + key);
        }

        public T Get<T>(string key)
        {
            var entry = Get(key);
            var array = entry.Value as JArray;
            return array != null ? array.ToObject<T>() : ((JObject) entry.Value).ToObject<T>();
        }
    }
}