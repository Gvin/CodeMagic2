using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Game.Items
{
    public interface IEquipableItem : IItem
    {
        int GetStatBonus(PlayerStats statType);

        int GetBonus(EquipableBonusType bonusType);
    }
}