using System;
using AspNetCore.Mvc.Fragments.Attributes;
using AspNetCore.Mvc.Fragments.Options;
using Newtonsoft.Json;

namespace AspNetCore.Mvc.Fragments.Converters
{
    internal class FragmentOptionsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IFragmentOptions);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize(reader, typeof(FragmentOptionsAttribute));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value, typeof(FragmentOptionsAttribute));
        }
    }
}