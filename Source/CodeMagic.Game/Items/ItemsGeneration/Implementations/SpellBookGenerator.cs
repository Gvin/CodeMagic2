using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Drawing;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.SpellBook;
using CodeMagic.Game.Items.ItemsGeneration.Implementations.Bonuses;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations;

public class SpellBookGenerator
{
    private const string WorldImageName = "ItemsOnGround_SpellBook";
    private const string DefaultName = "Spell Book";

    private readonly ISpellBooksConfiguration _configuration;
    private readonly IBonusesGenerator _bonusesGenerator;
    private readonly IImagesStorageService _imagesStorage;

    public SpellBookGenerator(ISpellBooksConfiguration configuration, IBonusesGenerator bonusesGenerator, IImagesStorageService imagesStorage)
    {
        _configuration = configuration;
        _bonusesGenerator = bonusesGenerator;
        _imagesStorage = imagesStorage;
    }

    public SpellBook GenerateSpellBook(ItemRareness rareness)
    {
        var config = GetConfiguration(rareness);
        var spellsCount = RandomHelper.GetRandomValue(config.MinSpells, config.MaxSpells);
        var bonusesCount = RandomHelper.GetRandomValue(config.MinBonuses, config.MaxBonuses);
        var inventoryImage = GenerateImage(out var mainColor);
        var worldImage = GetWorldImage(mainColor);

        var item = new SpellBook
        {
            Name = DefaultName,
            Key = Guid.NewGuid().ToString(),
            Description = GenerateDescription(config),
            BookSize = spellsCount,
            InventoryImage = inventoryImage,
            WorldImage = worldImage,
            Weight = _configuration.Weight,
            Rareness = rareness
        };

        _bonusesGenerator.GenerateBonuses(item, bonusesCount);

        return item;
    }

    private ISymbolsImage GetWorldImage(Color mainImageColor)
    {
        var imageInit = _imagesStorage.GetImage(WorldImageName);
        return ItemRecolorHelper.RecolorSpellBookGroundImage(imageInit, mainImageColor);
    }

    private ISymbolsImage GenerateImage(out Color mainColor)
    {
        var baseImageInit = _imagesStorage.GetImage(_configuration.Template);
        var symbolImageInit = _imagesStorage.GetImage(RandomHelper.GetRandomElement(_configuration.SymbolImages));
        var imageInit = SymbolsImage.Combine(baseImageInit, symbolImageInit);
        return ItemRecolorHelper.RecolorSpellBookImage(imageInit, out mainColor);
    }

    private string[] GenerateDescription(ISpellBookRarenessConfiguration config)
    {
        var result = new List<string>
        {
            RandomHelper.GetRandomElement(config.Description),
            RandomHelper.GetRandomElement(config.Description)
        };
        return result.Distinct().ToArray();
    }

    private ISpellBookRarenessConfiguration GetConfiguration(ItemRareness rareness)
    {
        var result = _configuration.Configuration.FirstOrDefault(config => config.Rareness == rareness);
        if (result == null)
            throw new ApplicationException($"Rareness configuration not found for spell book rareness: {rareness}");

        return result;
    }
}