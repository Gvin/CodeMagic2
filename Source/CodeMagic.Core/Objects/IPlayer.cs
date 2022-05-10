using System;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.Objects
{
    public interface IPlayer : ICreatureObject
    {
        event EventHandler Died;

        event EventHandler LeveledUp;

        int Mana { get; set; }

        int MaxMana { get; }

        int Stamina { get; set; }

        int HungerPercent { get; set; }

        int MaxCarryWeight { get; }

        int ManaRegeneration { get; }

        int ScrollReadingBonus { get; }

        int AccuracyBonus { get; }

        int DamageBonus { get; }

        IInventory Inventory { get; }

        IEquipment Equipment { get; }

        int MaxVisibilityRange { get; }

        int Experience { get; }

        int Level { get; }

        void AddExperience(int exp);

        bool IsKnownPotion(PotionType type);

        void MarkPotionKnown(PotionType type);

        void IncreaseStat(PlayerStats stat);

        int GetPureStat(PlayerStats stat);
    }
}