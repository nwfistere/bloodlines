using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloodlines.src.json
{
    public class CharacterDataModelWrapperConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(CharacterDataModelWrapper).IsAssignableFrom(objectType);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            CharacterDataModelWrapper wrapper = value as CharacterDataModelWrapper;

            JObject containerObj = JObject.FromObject(value);

            containerObj.Add(wrapper.characterType.ToString(), JToken.FromObject(wrapper.Character));
            containerObj.WriteTo(writer);
        }
    }
}
