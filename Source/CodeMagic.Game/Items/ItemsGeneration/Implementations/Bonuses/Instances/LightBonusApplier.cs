using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Bonuses;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Bonuses.Instances
{
    internal class LightBonusApplier : IBonusApplier
    {
        public const string BonusType = "LightBonus";
        private const string NamePrefix = "Shining";
        private const string KeyPower = "Power";

        private const string BonusCode = "light_bonus";

        public void Apply(IBonusConfiguration config, Item item, NameBuilder name)
        {
            if (item is not EquipableItem equipableItem)
                throw new ApplicationException(
                    $"{nameof(LightBonusApplier)} cannot be applied to item {item.GetType().Name}");

            var possiblePower = GetPossibleLightPower(config);
            var power = RandomHelper.GetRandomElement(possiblePower);

            equipableItem.LightPower = power;

            name.AddNamePrefix(BonusCode, NamePrefix);
            name.AddDescription(BonusCode, "It emits some light.");
        }

        private LightLevel[] GetPossibleLightPower(IBonusConfiguration config)
        {
            var powerStrings = config.Values[KeyPower].Replace(" ", "").Split(',');
            return powerStrings.Select(powerString => Enum.Parse(typeof(LightLevel), powerString)).Cast<LightLevel>().ToArray();
        }
    }
}