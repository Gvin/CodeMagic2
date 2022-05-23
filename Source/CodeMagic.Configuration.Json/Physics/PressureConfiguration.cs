using CodeMagic.Game.Configuration.Physics;
using CodeMagic.UI.Blazor.Configuration;
using Newtonsoft.Json;

namespace CodeMagic.Configuration.Json.Physics;

[Serializable]
public class PressureConfiguration : IPressureConfiguration
{
    public int MinValue { get; set; }

    public int MaxValue { get; set; }

    public int NormalValue { get; set; }

    public int NormalizeSpeed { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<PressureDamageConfiguration>))]
    public IPressureDamageConfiguration? LowPressureDamageConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<PressureDamageConfiguration>))]
    public IPressureDamageConfiguration? HighPressureDamageConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<PressureDamageConfiguration>))]
    public IPressureDamageConfiguration? ChangePressureDamageConfiguration { get; set; }
}

[Serializable]
public class PressureDamageConfiguration : IPressureDamageConfiguration
{
    public int PressureLevel { get; set; }

    public double DamageMultiplier { get; set; }
}
