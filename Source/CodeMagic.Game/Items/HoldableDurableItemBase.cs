using System;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Drawing;

namespace CodeMagic.Game.Items;

[Serializable]
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

    public virtual ISymbolsImage GetWorldImage(IImagesStorageService storage)
    {
        return WorldImage;
    }

    public virtual ISymbolsImage GetInventoryImage(IImagesStorageService storage)
    {
        return InventoryImage;
    }

    public ISymbolsImage GetEquippedImage(IPlayer player, IImagesStorageService imagesStorage)
    {
        if (string.Equals(Id, player.Equipment.RightHandItemId))
            return GetRightEquippedImage(imagesStorage);
        if (string.Equals(Id, player.Equipment.LeftHandItemId))
            return GetLeftEquippedImage(imagesStorage);

        throw new ApplicationException($"Trying to render item \"{Name}\" that not equipped on both left and right hand.");
    }

    protected virtual ISymbolsImage GetRightEquippedImage(IImagesStorageService storage)
    {
        return EquippedImageRight;
    }

    protected virtual ISymbolsImage GetLeftEquippedImage(IImagesStorageService storage)
    {
        return EquippedImageLeft;
    }

    public int EquippedImageOrder => 999;

    public abstract StyledLine[] GetDescription(IPlayer player);
}