using System;
using System.Collections.Generic;
using CodeMagic.Core.Area;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Game.Items
{
    [Serializable]
    public abstract class EquipableItem : Item, IEquipableItem, ILightSource, ILightObject
    {
        protected EquipableItem()
        {
            Bonuses = new Dictionary<EquipableBonusType, int>();
            StatBonuses = new Dictionary<PlayerStats, int>();
        }

        public Dictionary<EquipableBonusType, int> Bonuses { get; set; }

        public Dictionary<PlayerStats, int> StatBonuses { get; set; }

        public string[] Description { get; set; }

        public bool IsLightOn { get; set; }

        public LightLevel LightPower { get; set; }

        ILightSource[] ILightObject.LightSources => new ILightSource[] { this };

        public int GetBonus(EquipableBonusType bonusType)
        {
            if (!Bonuses.ContainsKey(bonusType))
                return 0;

            return Bonuses[bonusType];
        }

        public int GetStatBonus(PlayerStats statType)
        {
            if (!StatBonuses.ContainsKey(statType))
                return 0;

            return StatBonuses[statType];
        }
    }
}