using System.ComponentModel.DataAnnotations;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items;
using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Armor;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Description;
using CodeMagic.UI.Blazor.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CodeMagic.Configuration.Json.ItemGenerator;

[Serializable]
public class ArmorConfiguration : IArmorConfiguration
{
    [JsonConverter(typeof(FixedJsonTypeConverter<ArmorPieceConfiguration[]>))]
    public IArmorPieceConfiguration[]? ChestConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<ArmorPieceConfiguration[]>))]
    [Required]
    public IArmorPieceConfiguration[]? LeggingsConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<ArmorPieceConfiguration[]>))]
    public IArmorPieceConfiguration[]? HelmetConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<DescriptionConfiguration>))]
    public IDescriptionConfiguration? DescriptionConfiguration { get; set; }
}

[Serializable]
public class ArmorPieceConfiguration : IArmorPieceConfiguration
{
    [JsonProperty(Required = Required.AllowNull, NullValueHandling = NullValueHandling.Ignore)]
    public string? TypeName { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public ArmorClass Class { get; set; }

    public string[]? Images { get; set; }

    public string? EquippedImage { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<ArmorRarenessConfiguration[]>))]
    public IArmorRarenessConfiguration[]? RarenessConfigurations { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<WeightConfiguration[]>))]
    public IWeightConfiguration[]? Weight { get; set; }
}

[Serializable]
public class ArmorRarenessConfiguration : IArmorRarenessConfiguration
{
    [JsonConverter(typeof(StringEnumConverter))]
    public ItemRareness Rareness { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<ElementConfiguration[]>))]
    public IElementConfiguration[]? Protection { get; set; }

    public int MinBonuses { get; set; }

    public int MaxBonuses { get; set; }

    [JsonProperty(Required = Required.Always, ItemConverterType = typeof(StringEnumConverter))]
    public ItemMaterial[]? Materials { get; set; }
}