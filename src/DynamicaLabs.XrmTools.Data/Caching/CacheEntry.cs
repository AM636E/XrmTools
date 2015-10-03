using System;

namespace DynamicaLabs.XrmTools.Data.Caching
{
    public class CacheEntry
    {
        public DateTime InsertionDate { get; set; }
        public object Value { get; set; }
    }
}