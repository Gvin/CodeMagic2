using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Objects;

namespace CodeMagic.Game.Items
{
    public interface IShieldItem : IHoldableItem, IDurableItem
    {
        int BlocksDamage { get; }

        int ProtectChance { get; }

        int HitChancePenalty { get; }
    }

    [Serializable]
    public class ShieldItem : HoldableDurableItemBase, IShieldItem
    {
        public int BlocksDamage { get; set; }

        public int ProtectChance { get; set; }

        public int HitChancePenalty { get; set; }

        public override StyledLine[] GetDescription(IPlayer player)
        {
            var result = GetCharacteristicsDescription(player).ToList();

            result.Add(StyledLine.Empty);
            result.AddRange(TextHelper.ConvertDescription(Description));

            return result.ToArray();
        }

        private StyledLine[] GetCharacteristicsDescription(IPlayer player)
        {
            var rightHandShield = player.Inventory.GetItemById(player.Equipment.RightHandItemId) as IShieldItem;
            var leftHandShield = player.Inventory.GetItemById(player.Equipment.LeftHandItemId) as IShieldItem;

            var result = new List<StyledLine>();

            if (Equals(leftHandShield) || Equals(rightHandShield) || leftHandShield == null)
            {
                result.Add(TextHelper.GetWeightLine(Weight));
            }
            else
            {
                result.Add(TextHelper.GetCompareWeightLine(Weight, leftHandShield.Weight));
            }

            result.Add(StyledLine.Empty);

            result.Add(TextHelper.GetDurabilityLine(Durability, MaxDurability));

            result.Add(StyledLine.Empty);

            AddProtectionDescription(result, leftHandShield, rightHandShield);

            result.Add(StyledLine.Empty);

            if (Equals(rightHandShield) || Equals(leftHandShield) || leftHandShield == null)
            {
                TextHelper.AddBonusesDescription(this, null, result);
            }
            else
            {
                TextHelper.AddBonusesDescription(this, leftHandShield, result);
            }
            result.Add(StyledLine.Empty);

            TextHelper.AddLightBonusDescription(this, result);

            return result.ToArray();
        }

        private void AddProtectionDescription(List<StyledLine> descr, IShieldItem leftHandShield,
            IShieldItem rightHandShield)
        {
            var hitChanceLine = new StyledLine { "Protect Chance: " };
            if (Equals(rightHandShield) || Equals(leftHandShield) || leftHandShield == null)
            {
                hitChanceLine.Add(TextHelper.GetValueString(ProtectChance, "%", false));
            }
            else
            {
                hitChanceLine.Add(TextHelper.GetCompareValueString(ProtectChance, leftHandShield.ProtectChance, "%", false));
            }
            descr.Add(hitChanceLine);

            var blocksDamageLine = new StyledLine { "Blocks Damage: " };
            if (Equals(rightHandShield) || Equals(leftHandShield) || leftHandShield == null)
            {
                blocksDamageLine.Add(TextHelper.GetValueString(BlocksDamage, formatBonus: false));
            }
            else
            {
                blocksDamageLine.Add(TextHelper.GetCompareValueString(BlocksDamage, leftHandShield.BlocksDamage, formatBonus: false));
            }
            descr.Add(blocksDamageLine);

            var hitChancePenaltyLine = new StyledLine { "Hit Chance Penalty: " };
            if (Equals(rightHandShield) || Equals(leftHandShield) || leftHandShield == null)
            {
                hitChancePenaltyLine.Add(TextHelper.GetValueString(HitChancePenalty, "%", false));
            }
            else
            {
                hitChancePenaltyLine.Add(TextHelper.GetCompareValueString(HitChancePenalty, leftHandShield.HitChancePenalty, "%", false));
            }
            descr.Add(hitChancePenaltyLine);
        }
    }
}