using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.UI.Blazor.Configuration;
using Newtonsoft.Json;

namespace CodeMagic.Configuration.Json.ItemGenerator;

[Serializable]
public class LayersImagesConfiguration : ILayersImagesConfiguration
{
    [JsonConverter(typeof(FixedJsonTypeConverter<LayersImageSpriteConfiguration[]>))]
    public ILayersImageSpriteConfiguration[]? Sprites { get; set; }
}

[Serializable]
public class LayersImageSpriteConfiguration : ILayersImageSpriteConfiguration
{
    public int Index { get; set; }

    public string[]? Images { get; set; }
}