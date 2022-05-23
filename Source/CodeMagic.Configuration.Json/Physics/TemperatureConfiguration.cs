using CodeMagic.Game.Configuration.Physics;
using CodeMagic.UI.Blazor.Configuration;
using Newtonsoft.Json;

namespace CodeMagic.Configuration.Json.Physics;

[Serializable]
public class TemperatureConfiguration : ITemperatureConfiguration
{
    public int NormalValue { get; set; }

    public int MinValue { get; set; }

    public int MaxValue { get; set; }

    public int MaxTransferValue { get; set; }

    public double TransferValueToDifferenceMultiplier { get; set; }

    public int NormalizeSpeedInside { get; set; }

    public int NormalizeSpeedOutside { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<TemperatureDamageConfiguration>))]
    public ITemperatureDamageConfiguration? ColdDamageConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<TemperatureDamageConfiguration>))]
    public ITemperatureDamageConfiguration? HeatDamageConfiguration { get; set; }
}

[Serializable]
public class TemperatureDamageConfiguration : ITemperatureDamageConfiguration
{
    public int TemperatureLevel { get; set; }

    public double DamageMultiplier { get; set; }
}
