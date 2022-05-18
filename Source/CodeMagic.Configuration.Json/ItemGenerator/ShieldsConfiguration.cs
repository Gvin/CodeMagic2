using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Description;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Shield;
using CodeMagic.UI.Blazor.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CodeMagic.Configuration.Json.ItemGenerator;

[Serializable]
public class ShieldsConfiguration : IShieldsConfiguration
{
    [JsonConverter(typeof(FixedJsonTypeConverter<ShieldConfiguration>))]
    public IShieldConfiguration? SmallShieldConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<ShieldConfiguration>))]
    public IShieldConfiguration? MediumShieldConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<ShieldConfiguration>))]
    public IShieldConfiguration? BigShieldConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<DescriptionConfiguration>))]
    public IDescriptionConfiguration? DescriptionConfiguration { get; set; }
}

[Serializable]
public class ShieldConfiguration : IShieldConfiguration
{
    [JsonConverter(typeof(FixedJsonTypeConverter<LayersImagesConfiguration>))]
    public ILayersImagesConfiguration? Images { get; set; }

    public string? Name { get; set; }

    public string? WorldImage { get; set; }

    public string? EquippedImageRight { get; set; }

    public string? EquippedImageLeft { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<WeightConfiguration[]>))]
    public IWeightConfiguration[]? Weight { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<ShieldRarenessConfiguration[]>))]
    public IShieldRarenessConfiguration[]? RarenessConfiguration { get; set; }
}

[Serializable]
public class ShieldRarenessConfiguration : IShieldRarenessConfiguration
{
    public ItemRareness Rareness { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<IntervalConfiguration>))]
    public IIntervalConfiguration? BlocksDamage { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<IntervalConfiguration>))]
    public IIntervalConfiguration? ProtectChance { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<IntervalConfiguration>))]
    public IIntervalConfiguration? HitChancePenalty { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<IntervalConfiguration>))]
    public IIntervalConfiguration? Bonuses { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public ItemMaterial[]? Materials { get; set; }
}
