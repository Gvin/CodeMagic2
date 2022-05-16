using CodeMagic.Core.Items;
using CodeMagic.Game.Images;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items.Materials;

public class BlankScroll : Item, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
{
    public const string ItemKey = "scroll_blank";

    private const string WorldImageName = "ItemsOnGround_Scroll";
    private const string InventoryImageName = "Item_Scroll_Empty";

    public override string Key
    {
        get => ItemKey;
        set {}
    }

    public override ItemRareness Rareness
    {
        get => ItemRareness.Common;
        set {}
    }

    public override int Weight
    {
        get => 300;
        set {}
    }

    public override string Name
    {
        get => "Blank Scroll";
        set {}
    }

    public override bool Stackable => true;

    public ISymbolsImage GetWorldImage(IImagesStorageService storage)
    {
        return storage.GetImage(WorldImageName);
    }

    public ISymbolsImage GetInventoryImage(IImagesStorageService storage)
    {
        return storage.GetImage(InventoryImageName);
    }

    public StyledLine[] GetDescription(Player player)
    {
        return new[]
        {
            TextHelper.GetWeightLine(Weight),
            StyledLine.Empty,
            new StyledLine {new StyledString("An empty parchment scroll.", TextHelper.DescriptionTextColor) },
            new StyledLine {new StyledString("A spell can be written on it with mana.", TextHelper.DescriptionTextColor) },
            new StyledLine {new StyledString("You will need double mana amount for this.", TextHelper.DescriptionTextColor) }
        };
    }
}