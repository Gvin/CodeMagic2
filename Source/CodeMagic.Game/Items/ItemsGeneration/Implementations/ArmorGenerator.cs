using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Images;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Armor;
using CodeMagic.Game.Items.ItemsGeneration.Implementations.Bonuses;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations;

public class ArmorGenerator
{
    private const int MaxDurabilityPercent = 100;
    private const int MinDurabilityPercent = 30;

    private const string WorldImageNameChest = "ItemsOnGround_Armor_Chest";
    private const string WorldImageNameLeggings = "ItemsOnGround_Armor_Leggings";
    private const string WorldImageNameHelmet = "ItemsOnGround_Armor_Helmet";

    private readonly IArmorConfiguration _configuration;
    private readonly IImagesStorageService _imagesStorage;
    private readonly IBonusesGenerator _bonusesGenerator;

    public ArmorGenerator(IArmorConfiguration configuration, IBonusesGenerator bonusesGenerator, IImagesStorageService imagesStorage)
    {
        _configuration = configuration;
        _imagesStorage = imagesStorage;
        _bonusesGenerator = bonusesGenerator;
    }

    public ArmorItem GenerateArmor(ItemRareness rareness, ArmorClass armorClass)
    {
        var armorType = GenerateArmorType();
        var config = GetSpecificConfiguration(armorType, armorClass);
        var rarenessConfig = GetRarenessConfiguration(config, rareness);
        var material = RandomHelper.GetRandomElement(rarenessConfig.Materials);
        var inventoryImage = GetArmorImage(config, material);
        var worldImage = GetWorldImage(material, armorType);
        var equippedImage = GetEquippedImage(material, config);
        var protection = GenerateProtection(rarenessConfig.Protection);
        var name = GenerateName(material, config.TypeName, armorType);
        var description = GenerateDescription(rareness, material);
        var weightConfig = GetWeightConfiguration(config, material);
        var maxDurability = weightConfig.Durability;
        var bonusesCount = RandomHelper.GetRandomValue(rarenessConfig.MinBonuses, rarenessConfig.MaxBonuses);

        var durabilityPercent = RandomHelper.GetRandomValue(MinDurabilityPercent, MaxDurabilityPercent);
        var durability = Math.Min(maxDurability, (int)Math.Round(maxDurability * (durabilityPercent / 100d)));

        var result = new ArmorItem
        {
            Name = name,
            Key = Guid.NewGuid().ToString(),
            ArmorType = armorType,
            Description = description,
            InventoryImage = inventoryImage,
            WorldImage = worldImage,
            EquippedImage = equippedImage,
            Protection = protection,
            Rareness = rareness,
            Weight = weightConfig.Weight,
            MaxDurability = maxDurability,
            Durability = durability
        };

        _bonusesGenerator.GenerateBonuses(result, bonusesCount);

        return result;
    }

    private ISymbolsImage GetEquippedImage(ItemMaterial material, IArmorPieceConfiguration config)
    {
        if (string.IsNullOrEmpty(config.EquippedImage))
            return null;

        var image = _imagesStorage.GetImage(config.EquippedImage);
        return ItemRecolorHelper.RecolorItemImage(image, material);
    }

    private ISymbolsImage GetWorldImage(ItemMaterial material, ArmorType type)
    {
        var imageName = GetWorldImageName(type);
        var imageInit = _imagesStorage.GetImage(imageName);
        return ItemRecolorHelper.RecolorItemImage(imageInit, material);
    }

    private string GetWorldImageName(ArmorType type)
    {
        switch (type)
        {
            case ArmorType.Helmet:
                return WorldImageNameHelmet;
            case ArmorType.Chest:
                return WorldImageNameChest;
            case ArmorType.Leggings:
                return WorldImageNameLeggings;
            default:
                throw new ArgumentException($"Unknown armor type: {type}");
        }
    }

    private string GetArmorTypeName(ArmorType armorType)
    {
        switch (armorType)
        {
            case ArmorType.Helmet:
                return "Helmet";
            case ArmorType.Chest:
                return "Chest";
            case ArmorType.Leggings:
                return "Leggings";
            default:
                throw new ArgumentException($"Unknown armor type: {armorType}");
        }
    }

    private IWeightConfiguration GetWeightConfiguration(IArmorPieceConfiguration config, ItemMaterial material)
    {
        var result = config.Weight.FirstOrDefault(w => w.Material == material);
        if (result == null)
            throw new ApplicationException($"Weight configuration not found for armor material {material}");
        return result;
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
            throw new ApplicationException($"Text configuration not found for armor material: {material}");

        return RandomHelper.GetRandomElement(textConfig.Text);
    }

    private string GetRarenessDescription(ItemRareness rareness)
    {
        var textConfig = _configuration.DescriptionConfiguration.RarenessDescription.FirstOrDefault(config => config.Rareness == rareness);
        if (textConfig == null)
            throw new ApplicationException($"Text configuration not found for armor rareness: {rareness}");

        return RandomHelper.GetRandomElement(textConfig.Text);
    }

    private string GenerateName(ItemMaterial material, string typeName, ArmorType type)
    {
        var materialPrefix = NameGenerationHelper.GetMaterialPrefix(material);
        var armorTypeName = GetArmorTypeName(type);

        var builder = new List<string>();

        if (!string.IsNullOrEmpty(materialPrefix))
        {
            builder.Add(materialPrefix);

        }

        if (!string.IsNullOrEmpty(typeName))
        {
            builder.Add(typeName);
        }

        builder.Add(armorTypeName);

        return string.Join(" ", builder);
    }

    private Dictionary<Element, int> GenerateProtection(IElementConfiguration[] config)
    {
        return config.ToDictionary(c => c.Element, c => RandomHelper.GetRandomValue(c.MinValue, c.MaxValue));
    }

    private IArmorRarenessConfiguration GetRarenessConfiguration(IArmorPieceConfiguration config,
        ItemRareness rareness)
    {
        var result = config.RarenessConfigurations.FirstOrDefault(c => c.Rareness == rareness);
        if (result == null)
            throw new ApplicationException($"Rareness config not found for armor rareness {rareness}");

        return result;
    }

    private ISymbolsImage GetArmorImage(IArmorPieceConfiguration config, ItemMaterial material)
    {
        var imageName = RandomHelper.GetRandomElement(config.Images);
        var imageInit = _imagesStorage.GetImage(imageName);
        return ItemRecolorHelper.RecolorItemImage(imageInit, material);
    }

    private IArmorPieceConfiguration GetSpecificConfiguration(ArmorType type, ArmorClass armorClass)
    {
        var configurations = GetArmorTypeConfigurations(type);
        var classFilteredConfigurations = configurations
            .Where(config => config.Class == armorClass)
            .ToArray();
        return RandomHelper.GetRandomElement(classFilteredConfigurations);
    }

    private IArmorPieceConfiguration[] GetArmorTypeConfigurations(ArmorType type)
    {
        switch (type)
        {
            case ArmorType.Helmet:
                return _configuration.HelmetConfiguration;
            case ArmorType.Chest:
                return _configuration.ChestConfiguration;
            case ArmorType.Leggings:
                return _configuration.LeggingsConfiguration;
            default:
                throw new ArgumentException($"Unknown armor type: {type}");
        }
    }

    private ArmorType GenerateArmorType()
    {
        return (ArmorType) RandomHelper.GetRandomValue(0, 2);
    }
}