using System;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Drawing;

namespace CodeMagic.Game.Items.Materials;

[Serializable]
public class Wood : Item, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider, IFuelItem
{
    private const string ResourceKey = "resource_wood";
    private const string WorldImageName = "ItemsOnGround_Resource_Wood";
    private const string InventoryImageName = "Item_Resource_Wood";

    private const int DefaultMaxFuel = 120;

    public Wood()
    {
        FuelLeft = MaxFuel;
    }

    public override string Key => ResourceKey;

    public override ItemRareness Rareness => ItemRareness.Trash;

    public override int Weight => 2000;

    public override string Name => "Wood";

    public override bool Stackable => true;

    public bool CanIgnite => true;

    public int FuelLeft { get; set; }

    public int MaxFuel => DefaultMaxFuel;

    public int BurnTemperature => 700;

    public int IgnitionTemperature => 450;

    public ISymbolsImage GetWorldImage(IImagesStorageService storage)
    {
        return storage.GetImage(WorldImageName);
    }

    public ISymbolsImage GetInventoryImage(IImagesStorageService storage)
    {
        return storage.GetImage(InventoryImageName);
    }

    public StyledLine[] GetDescription(IPlayer player)
    {
        return new[]
        {
            TextHelper.GetWeightLine(Weight),
            StyledLine.Empty,
            new StyledLine {{"A big piece of wood.", TextHelper.DescriptionTextColor}},
            new StyledLine {{"It can be used as a fuel source.", TextHelper.DescriptionTextColor}}
        };
    }
}