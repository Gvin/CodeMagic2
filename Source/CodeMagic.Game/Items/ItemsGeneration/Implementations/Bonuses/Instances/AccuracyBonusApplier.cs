using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Bonuses;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Bonuses.Instances;

internal class AccuracyBonusApplier : IBonusApplier
{
    public const string BonusType = "AccuracyBonus";
    private const string BonusCode = "accuracy_bonus";

    private const string KeyMin = "Min";
    private const string KeyMax = "Max";

    public void Apply(IBonusConfiguration config, Item item, NameBuilder name)
    {
        if (item is not WeaponItem weaponItem)
            throw new ApplicationException(
                $"{nameof(AccuracyBonusApplier)} cannot be applied to item {item.GetType().Name}");

        var min = int.Parse(config.Values[KeyMin]);
        var max = int.Parse(config.Values[KeyMax]);
        var bonus = RandomHelper.GetRandomValue(min, max);

        if (bonus == 0)
            return;

        weaponItem.Accuracy = Math.Min(100, (int)Math.Round(weaponItem.Accuracy * (1 + bonus / 100d)));

        name.AddNamePrefix(BonusCode, "Balanced");
        name.AddDescription(BonusCode, GetDescription());
    }

    private string GetDescription()
    {
        return RandomHelper.GetRandomElement(
            "This item has good weight balance.", 
            "Good wight balance makes this item more accurate."
        );
    }
}