using System;
using System.Collections.Generic;
using System.Drawing;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Usable.Potions;

public sealed class Potion : Item, IUsableItem, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
{
    public PotionColor PotionColor { get; set; }

    public PotionType PotionType { get; set; }

    public PotionSize PotionSize { get; set; }

    public PotionData PotionData { get; set; }

    public override string Name => PotionData.GetName(PotionColor);

    public bool Use(IGameCore game)
    {
        PotionData.Use(game);

        if (!game.Player.IsKnownPotion(PotionType))
        {
            game.Player.MarkPotionKnown(PotionType);
        }

        return false;
    }
        
    public ISymbolsImage GetWorldImage(IImagesStorage storage)
    {
        var templateImage = storage.GetImage("ItemsOnGround_Potion");
        var palette = GetPotionPalette();
        return SymbolsImage.Recolor(templateImage, palette);
    }

    public ISymbolsImage GetInventoryImage(IImagesStorage storage)
    {
        var imageTemplateName = GetInventoryImageTemplateName();
        var templateImage = storage.GetImage(imageTemplateName);
        var palette = GetPotionPalette();
        return SymbolsImage.Recolor(templateImage, palette);
    }

    private Dictionary<Color, Color> GetPotionPalette()
    {
        switch (PotionColor)
        {
            case PotionColor.Red:
                return new Dictionary<Color, Color>
                {
                    {Color.FromArgb(255, 0, 0), Color.FromArgb(255, 0, 0)},
                    {Color.FromArgb(0, 255, 0), Color.FromArgb(196, 0, 0)}
                };
            case PotionColor.Blue:
                return new Dictionary<Color, Color>
                {
                    {Color.FromArgb(255, 0, 0), Color.FromArgb(79, 79, 255)},
                    {Color.FromArgb(0, 255, 0), Color.FromArgb(0, 0, 255)}
                };
            case PotionColor.Purple:
                return new Dictionary<Color, Color>
                {
                    {Color.FromArgb(255, 0, 0), Color.FromArgb(230, 0, 230)},
                    {Color.FromArgb(0, 255, 0), Color.FromArgb(128, 0, 128)}
                };
            case PotionColor.Green:
                return new Dictionary<Color, Color>
                {
                    {Color.FromArgb(255, 0, 0), Color.FromArgb(0, 210, 0)},
                    {Color.FromArgb(0, 255, 0), Color.FromArgb(0, 128, 0)}
                };
            case PotionColor.Orange:
                return new Dictionary<Color, Color>
                {
                    {Color.FromArgb(255, 0, 0), Color.FromArgb(255, 128, 0)},
                    {Color.FromArgb(0, 255, 0), Color.FromArgb(150, 70, 0)}
                };
            case PotionColor.Yellow:
                return new Dictionary<Color, Color>
                {
                    {Color.FromArgb(255, 0, 0), Color.FromArgb(255, 255, 0)},
                    {Color.FromArgb(0, 255, 0), Color.FromArgb(150, 150, 0)}
                };
            case PotionColor.White:
                return new Dictionary<Color, Color>
                {
                    {Color.FromArgb(255, 0, 0), Color.FromArgb(255, 255, 255)},
                    {Color.FromArgb(0, 255, 0), Color.FromArgb(192, 192, 192)}
                };
            case PotionColor.Gray:
                return new Dictionary<Color, Color>
                {
                    {Color.FromArgb(255, 0, 0), Color.FromArgb(192, 192, 192)},
                    {Color.FromArgb(0, 255, 0), Color.FromArgb(128, 128, 128)}
                };
            default:
                throw new ArgumentException($"Unknown potion color: {PotionColor}");
        }
    }

    private string GetInventoryImageTemplateName()
    {
        switch (PotionSize)
        {
            case PotionSize.Small:
                return "Item_Potion_Small";
            case PotionSize.Medium:
                return "Item_Potion";
            case PotionSize.Big:
                return "Item_Potion_Big";
            default:
                throw new ArgumentException($"Unknown potion size: {PotionSize}");
        }
    }

    public StyledLine[] GetDescription(Player player)
    {
        var description = new List<StyledLine>
        {
            TextHelper.GetWeightLine(Weight),
            StyledLine.Empty
        };
        description.AddRange(PotionData.GetDescription(player, PotionColor));

        return description.ToArray();
    }
}

public enum PotionSize
{
    Small,
    Medium,
    Big
}

public enum PotionColor
{
    Red,
    Blue,
    Purple,
    Green,
    Orange,
    Yellow,
    White,
    Gray
}