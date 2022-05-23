using CodeMagic.Configuration.Json.Common;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Configuration.Monsters;
using CodeMagic.Game.Objects.DecorativeObjects;
using CodeMagic.UI.Blazor.Configuration;
using Newtonsoft.Json;

namespace CodeMagic.Configuration.Json.Monsters;

[Serializable]
public class MonstersConfiguration : IMonstersConfiguration
{
    [JsonConverter(typeof(FixedJsonTypeConverter<MonsterConfiguration[]>))]
    public IMonsterConfiguration[]? Monsters { get; set; }
}

[Serializable]
public class MonsterConfiguration : IMonsterConfiguration
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public string? LogicPattern { get; set; }

    public string? Image { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<MonsterSpawnConfiguration>))]
    public IMonsterSpawnConfiguration? SpawnConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<MonsterExperienceConfiguration>))]
    public IMonsterExperienceConfiguration? Experience { get; set; }

    public ObjectSize Size { get; set; }

    public RemainsType RemainsType { get; set; }

    public RemainsType DamageMarkType { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<MonsterStatsConfiguration>))]
    public IMonsterStatsConfiguration? Stats { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<LootConfiguration>))]
    public ILootConfiguration? Loot { get; set; }
}

[Serializable]
public class MonsterSpawnConfiguration : IMonsterSpawnConfiguration
{
    public int MinLevel { get; set; }

    public int Force { get; set; }

    public string? Group { get; set; }
}

[Serializable]
public class MonsterExperienceConfiguration : IMonsterExperienceConfiguration
{
    public int Max { get; set; }

    public int Min { get; set; }
}
