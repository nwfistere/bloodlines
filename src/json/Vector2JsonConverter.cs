using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Bloodlines.src.json
{
    public class Vector2JsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Vector2).IsAssignableFrom(objectType);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            return jObject.ToObject<Vector2>();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is not Vector2 vector)
            {
                return;
            }

            writer.WriteStartObject();
            writer.WritePropertyName(nameof(Vector2.x));
            writer.WriteValue(vector.x);
            writer.WritePropertyName(nameof(Vector2.y));
            writer.WriteValue(vector.y);
            writer.WriteEndObject();
        }
    }
}
