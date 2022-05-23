using CodeMagic.Game.Configuration.Spells;
using CodeMagic.UI.Blazor.Configuration;
using Newtonsoft.Json;

namespace CodeMagic.Configuration.Json.Spells;

[Serializable]
public class SpellsConfiguration : ISpellsConfiguration
{
    [JsonConverter(typeof(FixedJsonTypeConverter<SpellConfiguration[]>))]
    public ISpellConfiguration[]? Spells { get; set; }
}

[Serializable]
public class SpellConfiguration : ISpellConfiguration
{
    public string? SpellType { get; set; }

    public double ManaCostMultiplier { get; set; }

    public int ManaCostPower { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<SpellConfigurationCustomValue[]>))]
    public ISpellConfigurationCustomValue[]? CustomValues { get; set; }
}

[Serializable]
public class SpellConfigurationCustomValue : ISpellConfigurationCustomValue
{
    public string? Key { get; set; }

    public string? Value { get; set; }
}
