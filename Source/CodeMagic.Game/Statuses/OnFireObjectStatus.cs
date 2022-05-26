using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Statuses;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.JournalMessages;

namespace CodeMagic.Game.Statuses;

[Serializable]
public class OnFireObjectStatus : IObjectStatus
{
    private const int CellTemperatureIncreaseMax = 100;

    public const string StatusType = "on_fire";

    public OnFireObjectStatus()
    {
    }

    public OnFireObjectStatus(OnFireObjectStatusConfiguration configuration)
    {
        BurningTemperature = configuration.BurningTemperature;
        BurnBeforeExtinguishCheck = configuration.BurnBeforeExtinguishCheck;

        BurnTime = 0;
    }

    public int BurningTemperature { get; set; }

    public int BurnBeforeExtinguishCheck { get; set; }

    public int BurnTime { get; set; }

    public bool Update(IDestroyableObject owner, Point position)
    {
        if (BurnTime >= BurnBeforeExtinguishCheck)
        {
            if (RandomHelper.CheckChance((int)Math.Round(owner.GetSelfExtinguishChance())))
            {
                return false;
            }
        }

        BurnTime++;

        var damage = Temperature.GetTemperatureDamage(GameConfigurationManager.Current.Physics.TemperatureConfiguration, BurningTemperature, out _);
        if (damage == 0)
            return true;

        CurrentGame.Journal.Write(new BurningDamageMessage(owner, damage), owner);
        owner.Damage(position, damage, Element.Fire);

        var cell = CurrentGame.Map.GetCell(position);
        var temperatureDiff = cell.Temperature() - BurningTemperature;
        if (temperatureDiff > 0)
        {
            var cellTemperatureIncrement = Math.Min(temperatureDiff, CellTemperatureIncreaseMax);
            cell.Environment.Temperature += cellTemperatureIncrement;
        }

        return true;
    }

    public IObjectStatus Merge(IObjectStatus oldStatus)
    {
        if (!(oldStatus is OnFireObjectStatus oldFire))
            throw new InvalidOperationException($"Unable to merge {nameof(OnFireObjectStatus)} status with {oldStatus.GetType().Name}");

        return oldFire;
    }

    public string Type => StatusType;
}

public class OnFireObjectStatusConfiguration
{
    public OnFireObjectStatusConfiguration()
    {
        BurningTemperature = 600;
    }

    public int BurningTemperature { get; set; }

    public int BurnBeforeExtinguishCheck { get; set; }
}