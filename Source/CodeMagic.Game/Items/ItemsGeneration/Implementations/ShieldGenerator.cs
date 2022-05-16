using System;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Images;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Shield;
using CodeMagic.Game.Items.ItemsGeneration.Implementations.Bonuses;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations;

public class ShieldGenerator
{
    private const int MaxDurabilityPercent = 100;
    private const int MinDurabilityPercent = 30;

    private readonly IShieldsConfiguration _configuration;
    private readonly IImagesStorageService _imagesStorage;
    private readonly IBonusesGenerator _bonusesGenerator;

    public ShieldGenerator(IShieldsConfiguration configuration, IBonusesGenerator bonusesGenerator, IImagesStorageService imagesStorage)
    {
        _configuration = configuration;
        _imagesStorage = imagesStorage;
        _bonusesGenerator = bonusesGenerator;
    }

    public IShieldItem GenerateShield(ItemRareness rareness)
    {
        var config = RandomHelper.GetRandomElement(
            _configuration.SmallShieldConfiguration,
            _configuration.MediumShieldConfiguration, 
            _configuration.BigShieldConfiguration);

        var rarenessConfig = GetRarenessConfiguration(config, rareness);
        var material = RandomHelper.GetRandomElement(rarenessConfig.Materials);
        var name = GenerateName(material, config.Name);
        var inventoryImage = GenerateInventoryImage(config, material);
        var worldImage = GetMaterialColoredImage(config.WorldImage, material);
        var equippedImageRight = GetMaterialColoredImage(config.EquippedImageRight, material);
        var equippedImageLeft = GetMaterialColoredImage(config.EquippedImageLeft, material);
        var bonusesCount = GetIntervalRandomValue(rarenessConfig.Bonuses);
        var weightConfiguration = GetWeightConfiguration(config, material);
        var maxDurability = weightConfiguration.Durability;
        var blocksDamage = GetIntervalRandomValue(rarenessConfig.BlocksDamage);
        var protectChance = GetIntervalRandomValue(rarenessConfig.ProtectChance);
        var hitChancePenalty = - GetIntervalRandomValue(rarenessConfig.HitChancePenalty); // Negative value
        var description = GenerateDescription(rareness, material);

        var item = new ShieldItem
        {
            Name = name,
            Key = Guid.NewGuid().ToString(),
            Rareness = rareness,
            InventoryImage = inventoryImage,
            WorldImage = worldImage,
            EquippedImageLeft = equippedImageLeft,
            EquippedImageRight = equippedImageRight,
            Weight = weightConfiguration.Weight,
            MaxDurability = maxDurability,
            BlocksDamage = blocksDamage,
            ProtectChance = protectChance,
            HitChancePenalty = hitChancePenalty,
            Description = description
        };

        _bonusesGenerator.GenerateBonuses(item, bonusesCount);

        var durabilityPercent = RandomHelper.GetRandomValue(MinDurabilityPercent, MaxDurabilityPercent);
        var durability = Math.Min(item.MaxDurability, (int)Math.Round(item.MaxDurability * (durabilityPercent / 100d)));
        item.Durability = durability;

        return item;
    }

    private string[] GenerateDescription(ItemRareness rareness, ItemMaterial material)
    {
        return new[]
        {
            GetMaterialDescription(material),
            GetRarenessDescription(rareness)
        };
    }

    private string GetMaterialDescription(ItemMaterial material)
    {
        var textConfig = _configuration.DescriptionConfiguration.MaterialDescription.FirstOrDefault(config => config.Material == material);
        if (textConfig == null)
            throw new ApplicationException($"Text configuration not found for shield material: {material}");

        return RandomHelper.GetRandomElement(textConfig.Text);
    }

    private string GetRarenessDescription(ItemRareness rareness)
    {
        var textConfig = _configuration.DescriptionConfiguration.RarenessDescription.FirstOrDefault(config => config.Rareness == rareness);
        if (textConfig == null)
            throw new ApplicationException($"Text configuration not found for shield rareness: {rareness}");

        return RandomHelper.GetRandomElement(textConfig.Text);
    }

    private int GetIntervalRandomValue(IIntervalConfiguration interval)
    {
        return RandomHelper.GetRandomValue(interval.Min, interval.Max);
    }

    private IWeightConfiguration GetWeightConfiguration(IShieldConfiguration config, ItemMaterial material)
    {
        var result = config.Weight.FirstOrDefault(conf => conf.Material == material);
        if (result == null)
            throw new ApplicationException($"No {config.Name} weight configuration for material: {material}");

        return result;
    }

    private ISymbolsImage GetMaterialColoredImage(string imageName, ItemMaterial material)
    {
        var image = _imagesStorage.GetImage(imageName);
        return ItemRecolorHelper.RecolorItemImage(image, material);
    }

    private IShieldRarenessConfiguration GetRarenessConfiguration(IShieldConfiguration config,
        ItemRareness rareness)
    {
        var result = config.RarenessConfiguration.FirstOrDefault(conf => conf.Rareness == rareness);
        if (result == null)
            throw new ApplicationException($"No shield rareness configuration for rareness: {rareness}");

        return result;
    }

    private string GenerateName(ItemMaterial material, string baseName)
    {
        var materialPrefix = NameGenerationHelper.GetMaterialPrefix(material);
        return $"{materialPrefix} {baseName}";
    }

    private ISymbolsImage GenerateInventoryImage(IShieldConfiguration config, ItemMaterial material)
    {
        var parts = config.Images.Sprites.OrderBy(sprite => sprite.Index)
            .Select(sprite => _imagesStorage.GetImage(RandomHelper.GetRandomElement(sprite.Images)))
            .Select(image => ItemRecolorHelper.RecolorItemImage(image, material))
            .ToArray();
        return MergeImages(parts);
    }

    private ISymbolsImage MergeImages(params ISymbolsImage[] images)
    {
        if (images.Length == 0)
            throw new ArgumentException("No images to merge.");

        var result = images[0];
        for (int index = 1; index < images.Length; index++)
        {
            result = SymbolsImage.Combine(result, images[index]);
        }

        return result;
    }
}