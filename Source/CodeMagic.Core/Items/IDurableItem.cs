using CodeMagic.Core.Items;

namespace CodeMagic.Game.Items;

public interface IDurableItem : IDecayItem
{
    int Durability { get; set; }

    int MaxDurability { get; }
}