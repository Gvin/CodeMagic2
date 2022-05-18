using CodeMagic.Game.Items.ItemsGeneration.Configuration;

namespace CodeMagic.Configuration.Json.ItemGenerator;

[Serializable]
public class IntervalConfiguration : IIntervalConfiguration
{
    public int Min { get; set; }

    public int Max { get; set; }
}