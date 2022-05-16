using Newtonsoft.Json;

namespace CodeMagic.UI.Blazor.Configuration;

public class FixedJsonTypeConverter<TType> : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        //assume we can convert to anything for now
        return true;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        //explicitly specify the type we want to create
        return serializer.Deserialize<TType>(reader)!;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        //use the default serialization - it works fine
        serializer.Serialize(writer, value);
    }
}