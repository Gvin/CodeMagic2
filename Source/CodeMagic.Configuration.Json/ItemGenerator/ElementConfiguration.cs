using CodeMagic.Core.Game;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CodeMagic.Configuration.Json.ItemGenerator;

[Serializable]
public class ElementConfiguration : IElementConfiguration
{
    [JsonConverter(typeof(StringEnumConverter))]
    public Element Element { get; set; }

    public int MinValue { get; set; }

    public int MaxValue { get; set; }
}