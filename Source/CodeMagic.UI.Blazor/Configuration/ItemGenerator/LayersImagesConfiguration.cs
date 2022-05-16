using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using Newtonsoft.Json;

namespace CodeMagic.UI.Blazor.Configuration.ItemGenerator;

[Serializable]
public class LayersImagesConfiguration : ILayersImagesConfiguration
{
    [JsonConverter(typeof(FixedJsonTypeConverter<LayersImageSpriteConfiguration>))]
    public ILayersImageSpriteConfiguration[]? Sprites { get; set; }
}

[Serializable]
public class LayersImageSpriteConfiguration : ILayersImageSpriteConfiguration
{
    public int Index { get; set; }

    public string[]? Images { get; set; }
}