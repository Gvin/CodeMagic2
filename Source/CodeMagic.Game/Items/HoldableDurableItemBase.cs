using System;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items
{
    public abstract class HoldableDurableItemBase : 
        DurableItem,
        IHoldableItem,
        IWorldImageProvider,
        IInventoryImageProvider,
        IEquippedImageProvider,
        IDescriptionProvider
    {
        public ISymbolsImage WorldImage { get; set; }

        public ISymbolsImage InventoryImage { get; set; }

        public ISymbolsImage EquippedImageRight { get; set; }

        public ISymbolsImage EquippedImageLeft { get; set; }

        public virtual ISymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return WorldImage;
        }

        public virtual ISymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return InventoryImage;
        }

        public ISymbolsImage GetEquippedImage(Player player, IImagesStorage imagesStorage)
        {
            if (Equals(player.Equipment.RightHandItem))
                return GetRightEquippedImage(imagesStorage);
            if (Equals(player.Equipment.LeftHandItem))
                return GetLeftEquippedImage(imagesStorage);

            throw new ApplicationException($"Trying to render item \"{Name}\" that not equipped on both left and right hand.");
        }

        protected virtual ISymbolsImage GetRightEquippedImage(IImagesStorage storage)
        {
            return EquippedImageRight;
        }

        protected virtual ISymbolsImage GetLeftEquippedImage(IImagesStorage storage)
        {
            return EquippedImageLeft;
        }

        public int EquippedImageOrder => 999;

        public abstract StyledLine[] GetDescription(Player player);
    }
}