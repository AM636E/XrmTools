using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DynamicaLabs.XrmTools.Core
{
    public class RequestAttributeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (objectType != typeof (RequestAttributeCollection))
            {
                return serializer.Deserialize(reader);
            }

            var request = new RequestAttributeCollection();
            var key = string.Empty;
            object value = null;
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                    {
                        continue;
                    }
                    case JsonToken.PropertyName:
                    {
                        if (!String.IsNullOrEmpty(key) && value != null)
                        {
                            request.Add(new RequestAttribute
                            {
                                Key = key,
                                Value = value
                            });
                            value = null;
                        }

                        key = reader.Value.ToString();
                        break;
                    }
                    case JsonToken.StartArray:
                    case JsonToken.EndArray:
                    {
                        value = serializer.Deserialize<JArray>(reader);
                        break;
                    }
                    case JsonToken.Integer:
                    case JsonToken.Boolean:
                    case JsonToken.Date:
                    case JsonToken.Float:
                    case JsonToken.String:
                    case JsonToken.Undefined:
                    {
                        value = reader.Value;
                        break;
                    }
                }
            }
            request.Add(new RequestAttribute
            {
                Key = key,
                Value = value
            });
            return request;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (RequestAttributeCollection);
        }
    }
}