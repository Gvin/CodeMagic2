﻿using System;
using System.Linq;
using System.Text.Json.Serialization;
using CodeMagic.Core.Statuses;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Configuration.Liquids;
using CodeMagic.Game.Objects.LiquidObjects;

namespace CodeMagic.Game.Statuses;

[Serializable]
public class OilyObjectStatus : PassiveObjectStatusBase, IBurningRelatedStatus
{
    private const string CustomValueOilyStatusLifeTime = "OilyStatus.LifeTime";
    private const string CustomValueOilyStatusCatchFireChanceModifier = "OilyStatus.CatchFireChanceModifier";
    private const string CustomValueOilyStatusSelfExtinguishChanceModifier = "OilyStatus.SelfExtinguishChanceModifier";

    public const string StatusType = "oily";

    public OilyObjectStatus()
        : base(GetMaxLifeTime())
    {
        var configuration = GetConfiguration();
        CatchFireChanceModifier = double.Parse(GetCustomConfigurationValue(configuration, CustomValueOilyStatusCatchFireChanceModifier));
        SelfExtinguishChanceModifier = double.Parse(GetCustomConfigurationValue(configuration, CustomValueOilyStatusSelfExtinguishChanceModifier));
    }

    private static ILiquidConfiguration GetConfiguration()
    {
        return GameConfigurationManager.GetLiquidConfiguration(OilLiquid.LiquidType);
    }

    private static int GetMaxLifeTime()
    {
        var configuration = GetConfiguration();
        return int.Parse(GetCustomConfigurationValue(configuration, CustomValueOilyStatusLifeTime));
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
            throw new ApplicationException($"Custom value {key} not found in the configuration for \"{OilLiquid.LiquidType}\".");

        return stringValue;
    }
}