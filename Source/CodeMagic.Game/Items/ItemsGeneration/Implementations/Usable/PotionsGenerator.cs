using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.Usable.Potions;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Usable
{
    public class PotionsGenerator : IUsableItemTypeGenerator
    {
        private readonly IPotionDataService _dataService;

        public PotionsGenerator(IPotionDataService dataService)
        {
            _dataService = dataService;
        }

        public IItem Generate(ItemRareness rareness)
        {
            var color = RandomHelper.GetRandomEnumValue<PotionColor>();
            var type = GameData.Current.GetPotionType(color);
            var size = GetSize(rareness);
            
            return new Potion
            {
                PotionColor = color,
                PotionType = type,
                PotionSize = size,
                Weight = GetWeight(size),
                PotionData = _dataService.GetPotionData(type, size),
                Key = GetKey(color)
            };
        }

        private static PotionSize GetSize(ItemRareness rareness)
        {
            switch (rareness)
            {
                case ItemRareness.Common:
                    return PotionSize.Small;
                case ItemRareness.Uncommon:
                    return PotionSize.Medium;
                case ItemRareness.Rare:
                    return PotionSize.Big;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rareness), rareness, null);
            }
        }

        private static int GetWeight(PotionSize size)
        {
            switch (size)
            {
                case PotionSize.Small:
                    return 200;
                case PotionSize.Medium:
                    return 500;
                case PotionSize.Big:
                    return 1000;
                default:
                    throw new ArgumentOutOfRangeException(nameof(size), size, null);
            }
        }

        private static string GetKey(PotionColor color)
        {
            switch (color)
            {
                case PotionColor.Red:
                    return "potion_red";
                case PotionColor.Blue:
                    return "potion_blue";
                case PotionColor.Purple:
                    return "potion_purple";
                case PotionColor.Green:
                    return "potion_green";
                case PotionColor.Orange:
                    return "potion_orange";
                case PotionColor.Yellow:
                    return "potion_yellow";
                case PotionColor.White:
                    return "potion_white";
                case PotionColor.Gray:
                    return "potion_gray";
                default:
                    throw new ArgumentException($"Unknown potion color: {color}", nameof(color));
            }
        }
    }
}