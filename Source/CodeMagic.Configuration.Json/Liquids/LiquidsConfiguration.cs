using CodeMagic.Game.Configuration.Liquids;
using CodeMagic.UI.Blazor.Configuration;
using Newtonsoft.Json;

namespace CodeMagic.Configuration.Json.Liquids;

[Serializable]
public class LiquidsConfiguration : ILiquidsConfiguration
{
    [JsonConverter(typeof(FixedJsonTypeConverter<LiquidConfiguration[]>))]
    public ILiquidConfiguration[]? LiquidsConfigurations { get; set; }
}

[Serializable]
public class LiquidConfiguration : ILiquidConfiguration
{
    public string? Type { get; set; }

    public int FreezingPoint { get; set; }

    public int BoilingPoint { get; set; }

    public int MinVolumeForEffect { get; set; }

    public int MaxVolumeBeforeSpread { get; set; }

    public int MaxSpreadVolume { get; set; }

    public int EvaporationMultiplier { get; set; }

    public double EvaporationTemperatureMultiplier { get; set; }

    public double CondensationTemperatureMultiplier { get; set; }

    public double FreezingTemperatureMultiplier { get; set; }

    public double MeltingTemperatureMultiplier { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<SteamConfiguration>))]
    public ISteamConfiguration? Steam { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<LiquidConfigurationCustomValue[]>))]
    public ILiquidConfigurationCustomValue[]? CustomValues { get; set; }
}

[Serializable]
public class SteamConfiguration : ISteamConfiguration
{
    public int PressureMultiplier { get; set; }

    public double ThicknessMultiplier { get; set; }

    public int MaxVolumeBeforeSpread { get; set; }

    public int MaxSpreadVolume { get; set; }
}

[Serializable]
public class LiquidConfigurationCustomValue : ILiquidConfigurationCustomValue
{
    public string? Key { get; set; }

    public string? Value { get; set; }
}
