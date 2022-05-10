using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Game.Items;

namespace CodeMagic.Core.Items;

public interface IEquipment
{
    string RightHandItemId { get; }

    string LeftHandItemId { get; }

    string SpellBookId { get; }

    bool RightHandItemEquipped { get; }

    bool LeftHandItemEquipped { get; }

    IHoldableItem GetLeftHandItem(IInventory playerInventory);

    IHoldableItem GetRightHandItem(IInventory playerInventory);

    int GetHitChanceBonus(IInventory playerInventory);

    IEquipableItem[] GetEquippedItems(IInventory playerInventory);

    IArmorItem GetEquipedArmor(ArmorType armorType, IInventory playerInventory);

    IWeaponItem GetEquipedWeapon(IInventory playerInventory, bool right);

    int GetProtection(Element element, IInventory playerInventory);

    int GetBonus(EquipableBonusType bonusType, IInventory playerInventory);

    int GetStatsBonus(PlayerStats statType, IInventory playerInventory);

    ILightSource[] GetLightSources(IInventory playerInventory);

    void EquipHoldable(IHoldableItem holdable, bool isRight);

    void EquipItem(IEquipableItem item);

    void UnequipItem(IEquipableItem item);

    bool IsEquiped(IEquipableItem item);
}