using CodeMagic.Game.Configuration.Physics;
using CodeMagic.UI.Blazor.Configuration;
using Newtonsoft.Json;

namespace CodeMagic.Configuration.Json.Physics;

[Serializable]
public class PhysicsConfiguration : IPhysicsConfiguration
{
    [JsonConverter(typeof(FixedJsonTypeConverter<TemperatureConfiguration>))]
    public ITemperatureConfiguration? TemperatureConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<PressureConfiguration>))]
    public IPressureConfiguration? PressureConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<MagicEnergyConfiguration>))]
    public IMagicEnergyConfiguration? MagicEnergyConfiguration { get; set; }
}
