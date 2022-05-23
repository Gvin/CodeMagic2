using CodeMagic.Game.Configuration.Physics;

namespace CodeMagic.Configuration.Json.Physics;

[Serializable]
public class MagicEnergyConfiguration : IMagicEnergyConfiguration
{
    public int MaxValue { get; set; }

    public int MaxTransferValue { get; set; }

    public int RegenerationValue { get; set; }

    public int DisturbanceStartLevel { get; set; }

    public int DisturbanceDamageStartLevel { get; set; }

    public int DisturbanceIncrement { get; set; }
}