using CodeMagic.Core.Game;
using CodeMagic.Game.Items;

namespace CodeMagic.Core.Items;

public interface IArmorItem : IEquipableItem, IDurableItem
{
    ArmorType ArmorType { get; }

    int GetProtection(Element element);
}

public enum ArmorType
{
    Helmet = 0,
    Chest = 1,
    Leggings = 2
}