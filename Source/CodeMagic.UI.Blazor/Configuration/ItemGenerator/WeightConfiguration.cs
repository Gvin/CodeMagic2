using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;

namespace CodeMagic.UI.Blazor.Configuration.ItemGenerator;

[Serializable]
public class WeightConfiguration : IWeightConfiguration
{
    public ItemMaterial Material { get; set; }

    public int Weight { get; set; }

    public int Durability { get; set; }
}