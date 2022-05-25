using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Drawing;
using CodeMagic.Game.Spells;

namespace CodeMagic.Game.Items;

public interface ISpellBook : IEquipableItem
{
    int BookSize { get; }

    BookSpell[] Spells { get; }
}

[Serializable]
public class SpellBook : EquipableItem, ISpellBook, IInventoryImageProvider, IDescriptionProvider, IWorldImageProvider
{
    private int _bookSize;

    public BookSpell[] Spells { get; set; }

    public int BookSize
    {
        get => _bookSize;
        set
        {
            _bookSize = value;
            Spells = new BookSpell[BookSize];
        }
    }

    public ISymbolsImage InventoryImage { get; set; }

    public ISymbolsImage WorldImage { get; set; }

    public override bool Stackable => false;

    public ISymbolsImage GetInventoryImage(IImagesStorageService storage)
    {
        return InventoryImage;
    }

    public StyledLine[] GetDescription(IPlayer player)
    {
        var equipedBook = player.Inventory.GetItemById<ISpellBook>(player.Equipment.SpellBookId);

        var result = new List<StyledLine>();

        if (equipedBook == null || Equals(equipedBook))
        {
            result.Add(TextHelper.GetWeightLine(Weight));
        }
        else
        {
            result.Add(TextHelper.GetCompareWeightLine(Weight, equipedBook.Weight));
        }

        result.Add(StyledLine.Empty);

        var capacityLine = new StyledLine { "Spells Capacity: " };
        if (equipedBook == null || Equals(equipedBook))
        {
            capacityLine.Add(TextHelper.GetValueString(BookSize, formatBonus: false));
        }
        else
        {
            capacityLine.Add(TextHelper.GetCompareValueString(BookSize, equipedBook.BookSize, formatBonus: false));
        }
        result.Add(capacityLine);

        result.Add(new StyledLine { $"Spells In Book: {Spells.Count(spell => spell != null)}" });

        result.Add(StyledLine.Empty);
        TextHelper.AddBonusesDescription(this, equipedBook, result);

        result.Add(StyledLine.Empty);
        TextHelper.AddLightBonusDescription(this, result);

        result.Add(StyledLine.Empty);

        result.AddRange(TextHelper.ConvertDescription(Description));

        return result.ToArray();
    }

    public ISymbolsImage GetWorldImage(IImagesStorageService storage)
    {
        return WorldImage;
    }
}