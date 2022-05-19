using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Description;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon;
using CodeMagic.UI.Blazor.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CodeMagic.Configuration.Json.ItemGenerator;

[Serializable]
public class WeaponsConfiguration : IWeaponsConfiguration
{
    [JsonConverter(typeof(FixedJsonTypeConverter<WeaponConfiguration>))]
    [JsonProperty(Required = Required.Always)]
    public IWeaponConfiguration? SwordsConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<WeaponConfiguration>))]
    [JsonProperty(Required = Required.Always)]
    public IWeaponConfiguration? DaggersConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<WeaponConfiguration>))]
    [JsonProperty(Required = Required.Always)]
    public IWeaponConfiguration? MacesConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<WeaponConfiguration>))]
    [JsonProperty(Required = Required.Always)]
    public IWeaponConfiguration? AxesConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<WeaponConfiguration>))]
    [JsonProperty(Required = Required.Always)]
    public IWeaponConfiguration? StaffsConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<DescriptionConfiguration>))]
    [JsonProperty(Required = Required.Always)]
    public IDescriptionConfiguration? DescriptionConfiguration { get; set; }
}

[Serializable]
public class WeaponConfiguration : IWeaponConfiguration
{
    [JsonConverter(typeof(FixedJsonTypeConverter<LayersImagesConfiguration>))]
    [JsonProperty(Required = Required.Always)]
    public ILayersImagesConfiguration? Images { get; set; }

    [JsonProperty(Required = Required.Always)]
    public string? EquippedImageRight { get; set; }

    [JsonProperty(Required = Required.Always)]
    public string? EquippedImageLeft { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<WeightConfiguration[]>))]
    [JsonProperty(Required = Required.Always)]
    public IWeightConfiguration[]? Weight { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<WeaponRarenessConfiguration[]>))]
    [JsonProperty(Required = Required.Always)]
    public IWeaponRarenessConfiguration[]? RarenessConfiguration { get; set; }
}

[Serializable]
public class WeaponRarenessConfiguration : IWeaponRarenessConfiguration
{
    [JsonConverter(typeof(StringEnumConverter))]
    public ItemRareness Rareness { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<ElementConfiguration[]>))]
    [JsonProperty(Required = Required.Always)]
    public IElementConfiguration[]? Damage { get; set; }

    public int MinMaxDamageDifference { get; set; }

    public int MinHitChance { get; set; }

    public int MaxHitChance { get; set; }

    public int MinBonuses { get; set; }

    public int MaxBonuses { get; set; }

    [JsonProperty(Required = Required.Always, ItemConverterType = typeof(StringEnumConverter))]
    public ItemMaterial[]? Materials { get; set; }
}