using CodeMagic.Core.Items;
using CodeMagic.Game.Items;
using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Armor;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Description;
using Newtonsoft.Json;

namespace CodeMagic.UI.Blazor.Configuration.ItemGenerator;

[Serializable]
public class ArmorConfiguration : IArmorConfiguration
{
    [JsonConverter(typeof(FixedJsonTypeConverter<ArmorPieceConfiguration[]>))]
    public IArmorPieceConfiguration[]? ChestConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<ArmorPieceConfiguration[]>))]
    public IArmorPieceConfiguration[]? LeggingsConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<ArmorPieceConfiguration[]>))]
    public IArmorPieceConfiguration[]? HelmetConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<DescriptionConfiguration>))]
    public IDescriptionConfiguration? DescriptionConfiguration { get; set; }
}

[Serializable]
public class ArmorPieceConfiguration : IArmorPieceConfiguration
{
    public string? TypeName { get; set; }

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
    public ItemRareness Rareness { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<ElementConfiguration[]>))]
    public IElementConfiguration[]? Protection { get; set; }

    public int MinBonuses { get; set; }

    public int MaxBonuses { get; set; }

    public ItemMaterial[]? Materials { get; set; }
}