using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Description;
using CodeMagic.UI.Blazor.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CodeMagic.Configuration.Json.ItemGenerator;

[Serializable]
public class DescriptionConfiguration : IDescriptionConfiguration
{
    [JsonConverter(typeof(FixedJsonTypeConverter<RarenessDescriptionConfiguration>))]
    public IRarenessDescriptionConfiguration[]? RarenessDescription { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<MaterialDescriptionConfiguration>))]
    public IMaterialDescriptionConfiguration[]? MaterialDescription { get; set; }
}

[Serializable]
public class RarenessDescriptionConfiguration : IRarenessDescriptionConfiguration
{
    [JsonConverter(typeof(StringEnumConverter))]
    public ItemRareness Rareness { get; set; }

    public string[]? Text { get; set; }
}

[Serializable]
public class MaterialDescriptionConfiguration : IMaterialDescriptionConfiguration
{
    [JsonConverter(typeof(StringEnumConverter))]
    public ItemMaterial Material { get; set; }

    public string[]? Text { get; set; }
}