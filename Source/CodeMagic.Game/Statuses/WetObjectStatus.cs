using System;
using System.Linq;
using System.Text.Json.Serialization;
using CodeMagic.Core.Statuses;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Configuration.Liquids;
using CodeMagic.Game.Objects.LiquidObjects;

namespace CodeMagic.Game.Statuses;

[Serializable]
public class WetObjectStatus : PassiveObjectStatusBase, IBurningRelatedStatus
{
    public const string StatusType = "wet";

    private const string CustomValueWetStatusLifeTime = "WetStatus.LifeTime";
    private const string CustomValueWetStatusCatchFileChanceModifier = "WetStatus.CatchFireChanceModifier";
    private const string CustomValueWetStatusSelfExtinguishChanceModifier = "WetStatus.SelfExtinguishChanceModifier";

    public WetObjectStatus()
        : base(GetMaxLifeTime())
    {
        var configuration = GetConfiguration();
        CatchFireChanceModifier = double.Parse(GetCustomConfigurationValue(configuration, CustomValueWetStatusCatchFileChanceModifier));
        SelfExtinguishChanceModifier = double.Parse(GetCustomConfigurationValue(configuration, CustomValueWetStatusSelfExtinguishChanceModifier));
    }

    private static ILiquidConfiguration GetConfiguration()
    {
        return GameConfigurationManager.GetLiquidConfiguration(WaterLiquid.LiquidType);
    }

    private static int GetMaxLifeTime()
    {
        var configuration = GetConfiguration();
        return int.Parse(GetCustomConfigurationValue(configuration, CustomValueWetStatusLifeTime));
    }

    public override string Type => StatusType;

    [JsonIgnore]
    public double CatchFireChanceModifier { get; }

    [JsonIgnore]
    public double SelfExtinguishChanceModifier { get; }

    private static string GetCustomConfigurationValue(ILiquidConfiguration configuration, string key)
    {
        var stringValue = configuration.CustomValues
            .FirstOrDefault(value => string.Equals(value.Key, key))?.Value;
        if (string.IsNullOrEmpty(stringValue))
            throw new ApplicationException($"Custom value {key} not found in the configuration for \"{WaterLiquid.LiquidType}\".");

        return stringValue;
    }
}