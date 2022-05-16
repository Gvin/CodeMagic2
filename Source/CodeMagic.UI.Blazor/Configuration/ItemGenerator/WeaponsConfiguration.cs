using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Description;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon;
using Newtonsoft.Json;

namespace CodeMagic.UI.Blazor.Configuration.ItemGenerator;

[Serializable]
public class WeaponsConfiguration : IWeaponsConfiguration
{
    [JsonConverter(typeof(FixedJsonTypeConverter<WeaponConfiguration>))]
    public IWeaponConfiguration? SwordsConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<WeaponConfiguration>))]
    public IWeaponConfiguration? DaggersConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<WeaponConfiguration>))]
    public IWeaponConfiguration? MacesConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<WeaponConfiguration>))]
    public IWeaponConfiguration? AxesConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<WeaponConfiguration>))]
    public IWeaponConfiguration? StaffsConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<DescriptionConfiguration>))]
    public IDescriptionConfiguration? DescriptionConfiguration { get; set; }
}

[Serializable]
public class WeaponConfiguration : IWeaponConfiguration
{
    [JsonConverter(typeof(FixedJsonTypeConverter<LayersImagesConfiguration>))]
    public ILayersImagesConfiguration? Images { get; set; }

    public string? EquippedImageRight { get; set; }

    public string? EquippedImageLeft { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<WeightConfiguration[]>))]
    public IWeightConfiguration[]? Weight { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<WeaponRarenessConfiguration[]>))]
    public IWeaponRarenessConfiguration[]? RarenessConfiguration { get; set; }
}

[Serializable]
public class WeaponRarenessConfiguration : IWeaponRarenessConfiguration
{
    public ItemRareness Rareness { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<ElementConfiguration[]>))]
    public IElementConfiguration[]? Damage { get; set; }

    public int MinMaxDamageDifference { get; set; }

    public int MinHitChance { get; set; }

    public int MaxHitChance { get; set; }

    public int MinBonuses { get; set; }

    public int MaxBonuses { get; set; }

    public ItemMaterial[]? Materials { get; set; }
}