using System;
using CodeMagic.Core.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Materials
{
    [Serializable]
    public class Stone : Item, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
    {
        private const string ResourceKey = "resource_stone";
        private const string WorldImageName = "Decoratives_Stones_Small";
        private const string InventoryImageName = "Item_Resource_Stone";

        public override string Key => ResourceKey;

        public override ItemRareness Rareness => ItemRareness.Trash;

        public override int Weight => 3000;

        public override string Name => "Stone";

        public override bool Stackable => true;

        public ISymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(WorldImageName);
        }

        public ISymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage(InventoryImageName);
        }

        public StyledLine[] GetDescription(Player player)
        {
            return new[]
            {
                TextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine {{"A normal medium size stone.", TextHelper.DescriptionTextColor}}
            };
        }
    }
}