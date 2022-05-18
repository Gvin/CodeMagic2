using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CodeMagic.Configuration.Json.ItemGenerator;

[Serializable]
public class WeightConfiguration : IWeightConfiguration
{
    [JsonConverter(typeof(StringEnumConverter))]
    public ItemMaterial Material { get; set; }

    public int Weight { get; set; }

    public int Durability { get; set; }
}