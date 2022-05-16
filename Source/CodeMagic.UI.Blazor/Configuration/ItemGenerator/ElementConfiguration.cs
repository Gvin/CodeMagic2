using CodeMagic.Core.Game;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;

namespace CodeMagic.UI.Blazor.Configuration.ItemGenerator;

[Serializable]
public class ElementConfiguration : IElementConfiguration
{
    public Element Element { get; set; }

    public int MinValue { get; set; }

    public int MaxValue { get; set; }
}