using CodeMagic.Core.Items;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Items;
using CodeMagic.UI.Blazor.Configuration;
using Newtonsoft.Json;

namespace CodeMagic.Configuration.Json.Common;

[Serializable]
public class LootConfiguration : ILootConfiguration
{
    [JsonConverter(typeof(FixedJsonTypeConverter<StandardLootConfiguration>))]
    public IStandardLootConfiguration? Weapon { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<StandardLootConfiguration>))]
    public IStandardLootConfiguration? Shield { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<ArmorLootConfiguration>))]
    public IArmorLootConfiguration? Armor { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<StandardLootConfiguration>))]
    public IStandardLootConfiguration? SpellBook { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<StandardLootConfiguration>))]
    public IStandardLootConfiguration? Usable { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<StandardLootConfiguration>))]
    public IStandardLootConfiguration? Resource { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<SimpleLootConfiguration>))]
    public ISimpleLootConfiguration? Food { get; set; }
}

[Serializable]
public class ArmorLootConfiguration : StandardLootConfiguration, IArmorLootConfiguration
{
    [JsonConverter(typeof(FixedJsonTypeConverter<ChanceConfiguration<ArmorClass>[]>))]
    public IChanceConfiguration<ArmorClass>[]? Class { get; set; }
}

[Serializable]
public class StandardLootConfiguration : SimpleLootConfiguration, IStandardLootConfiguration
{
    [JsonConverter(typeof(FixedJsonTypeConverter<ChanceConfiguration<ItemRareness>[]>))]
    public IChanceConfiguration<ItemRareness>[]? Rareness { get; set; }
}

[Serializable]
public class SimpleLootConfiguration : ISimpleLootConfiguration
{
    [JsonConverter(typeof(FixedJsonTypeConverter<ChanceConfiguration<int>[]>))]
    public IChanceConfiguration<int>[]? Count { get; set; }
}

[Serializable]
public class ChanceConfiguration<T> : IChanceConfiguration<T>
{
    public ChanceConfiguration(int chance, T value)
    {
        Chance = chance;
        Value = value;
    }

    public ChanceConfiguration()
    {
    }

    public int Chance { get; set; }

    public T Value { get; set; } = default!;

    public static IChanceConfiguration<T>[] Create(params (int chance, T value)[] chances)
    {
        return chances
            .Select(chanceGroup => 
                new ChanceConfiguration<T>(chanceGroup.chance, chanceGroup.value))
            .Cast<IChanceConfiguration<T>>()
            .ToArray();
    }
}
