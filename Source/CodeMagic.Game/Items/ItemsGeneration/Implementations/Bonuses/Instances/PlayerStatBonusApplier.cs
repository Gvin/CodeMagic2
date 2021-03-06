using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Bonuses;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Bonuses.Instances
{
    internal class PlayerStatBonusApplier : IBonusApplier
    {
        public const string BonusType = "PlayerStatBonus";

        private const string KeyMin = "Min";
        private const string KeyMax = "Max";

        public void Apply(IBonusConfiguration config, Item item, NameBuilder name)
        {
            if (item is not EquipableItem equipableItem)
                throw new ApplicationException(
                    $"{nameof(PlayerStatBonusApplier)} cannot be applied to item {item.GetType().Name}");

            var bonusType = GenerateStat();

            var min = int.Parse(config.Values[KeyMin]);
            var max = int.Parse(config.Values[KeyMax]);
            var bonus = RandomHelper.GetRandomValue(min, max);

            if (bonus == 0)
                return;

            if (!equipableItem.StatBonuses.ContainsKey(bonusType))
            {
                equipableItem.StatBonuses.Add(bonusType, 0);
            }
            equipableItem.StatBonuses[bonusType] += bonus;

            var bonusCode = GetBonusCode(bonusType);
            name.AddNamePostfix(bonusCode, GetNamePostfix(bonusType));
            name.AddDescription(bonusCode, GetBonusText(bonusType));
        }

        private static string GetBonusCode(PlayerStats stat)
        {
            return $"player_stat_bonus_{stat}";
        }

        private static string GetBonusText(PlayerStats stat)
        {
            var statName = TextHelper.GetStatName(stat);
            return $"It increases your {statName}.";
        }

        private static PlayerStats GenerateStat()
        {
            return RandomHelper.GetRandomEnumValue<PlayerStats>();
        }

        private static string GetNamePostfix(PlayerStats statType)
        {
            return TextHelper.GetStatName(statType);
        }
    }
}