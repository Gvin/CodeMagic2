using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Description;
using Newtonsoft.Json;

namespace CodeMagic.UI.Blazor.Configuration.ItemGenerator;

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
    public ItemRareness Rareness { get; set; }

    public string[]? Text { get; set; }
}

[Serializable]
public class MaterialDescriptionConfiguration : IMaterialDescriptionConfiguration
{
    public ItemMaterial Material { get; set; }

    public string[]? Text { get; set; }
}