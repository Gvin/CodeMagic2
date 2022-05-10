using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items
{
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
    }

    [Serializable]
    public class Equipment : IEquipment
    {
        public static readonly WeaponItem Fists = new()
        {
            MaxDamage = new Dictionary<Element, int> {{Element.Blunt, 2}},
            MinDamage = new Dictionary<Element, int> {{Element.Blunt, 0}},
            Accuracy = 50,
            Name = "Fists",
            Key = "default_fists",
            Rareness = ItemRareness.Trash,
            Weight = 0
        };

        public Equipment()
        {
            Armor = new Dictionary<ArmorType, string>
            {
                {ArmorType.Helmet, null},
                {ArmorType.Chest, null},
                {ArmorType.Leggings, null}
            };
        }

        public Dictionary<ArmorType, string> Armor { get; set; }

        public string RightHandItemId { get; set; }

        public string LeftHandItemId { get; set; }

        public string SpellBookId { get; set; }

        public bool RightHandItemEquipped => !string.IsNullOrEmpty(RightHandItemId);

        public bool LeftHandItemEquipped => !string.IsNullOrEmpty(LeftHandItemId);

        public IHoldableItem GetLeftHandItem(IInventory playerInventory)
        {
            return playerInventory.GetItemById<IHoldableItem>(LeftHandItemId) ?? Fists;
        }

        public IHoldableItem GetRightHandItem(IInventory playerInventory)
        {
            return playerInventory.GetItemById<IHoldableItem>(RightHandItemId) ?? Fists;
        }

        public int GetHitChanceBonus(IInventory playerInventory) => 
            GetEquippedItems(playerInventory).OfType<ShieldItem>().Sum(shield => shield.HitChancePenalty);

        public IWeaponItem GetEquipedWeapon(IInventory playerInventory, bool right)
        {
            if (right)
            {
                if (RightHandItemEquipped)
                {
                    return playerInventory.GetItemById<IWeaponItem>(RightHandItemId, true);
                }

                return Fists;
            }
            else
            {
                if (LeftHandItemEquipped)
                {
                    return playerInventory.GetItemById<IWeaponItem>(LeftHandItemId, true);
                }

                return Fists;
            }
        }

        public IEquipableItem[] GetEquippedItems(IInventory playerInventory)
        {
            var leftHandItem = playerInventory.GetItemById<IEquipableItem>(LeftHandItemId);
            var rightHandItem = playerInventory.GetItemById<IEquipableItem>(RightHandItemId);

            var spellBook = playerInventory.GetItemById<IEquipableItem>(SpellBookId);

            var result = new List<IEquipableItem>
            {
                spellBook,
                leftHandItem,
                rightHandItem
            };

            var armor = Armor.Values
                .Select(armorId => playerInventory.GetItemById<IEquipableItem>(armorId));

            result.AddRange(armor);

            return result.Where(item => item != null).ToArray();
        }

        public IArmorItem GetEquipedArmor(ArmorType armorType, IInventory playerInventory)
        {
            var itemId = Armor[armorType];
            return playerInventory.GetItemById<IArmorItem>(itemId);
        }

        public void EquipHoldable(IHoldableItem holdable, bool isRight)
        {
            if (isRight)
            {
                RightHandItemId = holdable.Id;
            }
            else
            {
                LeftHandItemId = holdable.Id;
            }
        }

        public void EquipItem(IEquipableItem item)
        {
            switch (item)
            {
                case IHoldableItem:
                    throw new ArgumentException($"Weapon items should be equipped with {nameof(EquipHoldable)}");
                case IArmorItem armorItem:
                    EquipArmor(armorItem);
                    break;
                case SpellBook spellBookItem:
                    EquipSpellBook(spellBookItem);
                    break;
                default:
                    throw new ArgumentException($"Equipable item type is not supported: {item.GetType().FullName}");
            }
        }

        public void UnequipItem(IEquipableItem item)
        {
            switch (item)
            {
                case IHoldableItem holdableItem:
                    UnequipHoldable(holdableItem);
                    break;
                case IArmorItem armorItem:
                    UnequipArmor(armorItem.ArmorType);
                    break;
                case ISpellBook:
                    UnequipSpellBook();
                    break;
                default:
                    throw new ArgumentException($"Equipable item type is not supported: {item.GetType().FullName}");
            }
        }

        private void UnequipArmor(ArmorType type)
        {
            var armor = Armor[type];
            if (armor != null)
            {
                Armor[type] = null;
            }
        }

        private void UnequipHoldable(IHoldableItem holdable)
        {
            if (RightHandItemEquipped && string.Equals(RightHandItemId, holdable.Id))
            {
                RightHandItemId = null;
            }
            else if (LeftHandItemEquipped && string.Equals(LeftHandItemId, holdable.Id))
            {
                LeftHandItemId = null;
            }
            else
            {
                throw new ArgumentException($"Weapon {holdable.Key} is not equiped");
            }
        }

        private void UnequipSpellBook()
        {
            if (SpellBookId != null)
            {
                SpellBookId = null;
            }
        }

        private void EquipArmor(IArmorItem newArmor)
        {
            UnequipArmor(newArmor.ArmorType);
            Armor[newArmor.ArmorType] = newArmor.Id;
        }

        private void EquipSpellBook(ISpellBook newSpellBook)
        {
            UnequipSpellBook();
            SpellBookId = newSpellBook.Id;
        }

        public bool IsEquiped(IEquipableItem item)
        {
            switch (item)
            {
                case IHoldableItem holdableItem:
                    return (RightHandItemEquipped && string.Equals(RightHandItemId, holdableItem.Id)) || 
                           (LeftHandItemEquipped && string.Equals(LeftHandItemId, holdableItem.Id));
                case IArmorItem armorItem:
                    return Armor[armorItem.ArmorType] != null && string.Equals(Armor[armorItem.ArmorType], item.Id);
                case ISpellBook spellBookItem:
                    return !string.IsNullOrEmpty(SpellBookId) && string.Equals(SpellBookId, spellBookItem.Id);
                default:
                    throw new ArgumentException($"Equipable item type is not supported: {item.GetType().FullName}");
            }
        }

        public int GetProtection(Element element, IInventory playerInventory)
        {
            return Armor.Values
                .Where(armorId => !string.IsNullOrEmpty(armorId))
                .Sum(armorId => playerInventory.GetItemById<IArmorItem>(armorId, true).GetProtection(element));
        }

        public int GetBonus(EquipableBonusType bonusType, IInventory playerInventory)
        {
            return GetEquippedItems(playerInventory).Sum(item => item.GetBonus(bonusType));
        }

        public int GetStatsBonus(PlayerStats statType, IInventory playerInventory)
        {
            return GetEquippedItems(playerInventory).Sum(item => item.GetStatBonus(statType));
        }

        public ILightSource[] GetLightSources(IInventory playerInventory)
        {
            return GetEquippedItems(playerInventory).OfType<ILightSource>().ToArray();
        }
    }
}