using CodeMagic.Core.Game;
using CodeMagic.Game.Configuration.Monsters;
using CodeMagic.UI.Blazor.Configuration;
using Newtonsoft.Json;

namespace CodeMagic.Configuration.Json.Monsters;

[Serializable]
public class MonsterStatsConfiguration : IMonsterStatsConfiguration
{
    public int MaxHealth { get; set; }

    public int MinHealth { get; set; }

    public float Speed { get; set; }

    public int CatchFireChanceMultiplier { get; set; }

    public int SelfExtinguishChanceMultiplier { get; set; }

    public int VisibilityRange { get; set; }

    public int Accuracy { get; set; }

    public int DodgeChance { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<MonsterProtectionConfiguration[]>))]
    public IMonsterProtectionConfiguration[]? Protection { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<MonsterDamageConfiguration[]>))]
    public IMonsterDamageConfiguration[]? Damage { get; set; }

    public string[]? StatusesImmunity { get; set; }

    public int ShieldBlockChance { get; set; }

    public int ShieldBlocksDamage { get; set; }
}

[Serializable]
public class MonsterProtectionConfiguration : IMonsterProtectionConfiguration
{
    public Element Element { get; set; }

    public int Value { get; set; }
}

[Serializable]
public class MonsterDamageConfiguration : IMonsterDamageConfiguration
{
    public Element Element { get; set; }

    public int MinValue { get; set; }

    public int MaxValue { get; set; }
}
