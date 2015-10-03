namespace DynamicaLabs.XrmTools.Core
{
    public class RequestAttribute
    {
        public RequestAttribute()
        {
        }

        public RequestAttribute(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public object Value { get; set; }
    }
}