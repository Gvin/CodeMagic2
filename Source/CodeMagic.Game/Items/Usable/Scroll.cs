using System;
using System.Linq;
using CodeMagic.Game.Images;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items.Usable;

[Serializable]
public class Scroll : ScrollBase
{
    private const string ImageWorld = "ItemsOnGround_Scroll";

    private const string ImageInventory1 = "Item_Scroll_New_V1";
    private const string ImageInventory2 = "Item_Scroll_New_V2";
    private const string ImageInventory3 = "Item_Scroll_New_V3";

    public Scroll()
    {
    }

    public Scroll(string code)
    {
        Code = code;
        InventoryImageName = GetInventoryImageName(code);
    }

    public string InventoryImageName { get; set; }

    private static string GetInventoryImageName(string code)
    {
        var letterA = code.Count(c => char.ToLower(c) == 'a');
        var letterB = code.Count(c => char.ToLower(c) == 'b');
        var letterC = code.Count(c => char.ToLower(c) == 'c');

        if (letterA > letterB && letterA > letterC)
            return ImageInventory1;
        if (letterB > letterA && letterB > letterC)
            return ImageInventory2;
        return ImageInventory3;
    }

    public override ISymbolsImage GetWorldImage(IImagesStorageService storage)
    {
        return storage.GetImage(ImageWorld);
    }

    public override ISymbolsImage GetInventoryImage(IImagesStorageService storage)
    {
        if (string.IsNullOrEmpty(InventoryImageName))
        {
            InventoryImageName = GetInventoryImageName(Code);
        }

        return storage.GetImage(InventoryImageName);
    }

    public override StyledLine[] GetDescription(Player player)
    {
        return new[]
        {
            TextHelper.GetWeightLine(Weight),
            StyledLine.Empty,
            new StyledLine {$"Spell Name: {SpellName}"},
            new StyledLine {"Spell Mana: ", new StyledString(Mana.ToString(), TextHelper.ManaColor)}, 
            StyledLine.Empty, 
            new StyledLine {new StyledString("A new scroll that you have created. A single use item.", TextHelper.DescriptionTextColor) },
            new StyledLine {new StyledString("It can be used to cast a spell without mana loss.", TextHelper.DescriptionTextColor) }
        };
    }
}