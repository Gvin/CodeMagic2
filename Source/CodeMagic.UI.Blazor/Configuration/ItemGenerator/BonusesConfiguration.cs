using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Bonuses;
using Newtonsoft.Json;

namespace CodeMagic.UI.Blazor.Configuration.ItemGenerator;

[Serializable]
public class BonusesConfiguration : IBonusesConfiguration
{
    [JsonConverter(typeof(FixedJsonTypeConverter<ItemGroupBonusesConfiguration[]>))]
    public IItemGroupBonusesConfiguration[]? Groups { get; set; }
}

[Serializable]
public class ItemGroupBonusesConfiguration : IItemGroupBonusesConfiguration
{
    public string? Type { get; set; }

    public string? InheritFrom { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<BonusRarenessConfiguration[]>))]
    public IBonusRarenessConfiguration[]? Configuration { get; set; }
}

[Serializable]
public class BonusRarenessConfiguration : IBonusRarenessConfiguration
{
    public ItemRareness Rareness { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<BonusConfiguration[]>))]
    public IBonusConfiguration[]? Bonuses { get; set; }
}

[Serializable]
public class BonusConfiguration : IBonusConfiguration
{
    public string? Type { get; set; }

    public Dictionary<string, string>? Values { get; set; }
}
