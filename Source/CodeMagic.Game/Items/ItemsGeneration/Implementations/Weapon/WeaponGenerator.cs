using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon;
using CodeMagic.Game.Items.ItemsGeneration.Implementations.Bonuses;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Weapon
{
    internal class WeaponGenerator : IWeaponGenerator
    {
        private const int MaxDurabilityPercent = 100;
        private const int MinDurabilityPercent = 30;

        private readonly IImagesStorage _imagesStorage;
        private readonly string _baseName;
        private readonly string _worldImageName;
        private readonly IWeaponConfiguration _configuration;
        private readonly IWeaponsConfiguration _configurations;
        private readonly IBonusesGenerator _bonusesGenerator;

        public WeaponGenerator(
            string baseName, 
            string worldImageName,
            IWeaponConfiguration configuration,
            IWeaponsConfiguration configurations, 
            IBonusesGenerator bonusesGenerator, 
            IImagesStorage imagesStorage)
        {
            this._configurations = configurations;
            this._imagesStorage = imagesStorage;
            this._bonusesGenerator = bonusesGenerator;
            this._baseName = baseName;
            this._worldImageName = worldImageName;
            this._configuration = configuration;
        }

        public IWeaponItem GenerateWeapon(ItemRareness rareness)
        {
            var rarenessConfiguration = GetRarenessConfiguration(rareness);
            var material = RandomHelper.GetRandomElement(rarenessConfiguration.Materials);
            var inventoryImage = GenerateImage(material);
            var worldImage = GetMaterialColoredImage(_worldImageName, material);
            var equippedRightImage = GetMaterialColoredImage(_configuration.EquippedImageRight, material);
            var equippedLeftImage = GetMaterialColoredImage(_configuration.EquippedImageLeft, material);
            var maxDamage = GenerateMaxDamage(rarenessConfiguration);
            var minDamage = maxDamage.ToDictionary(pair => pair.Key, pair => pair.Value - rarenessConfiguration.MinMaxDamageDifference);
            var hitChance = RandomHelper.GetRandomValue(rarenessConfiguration.MinHitChance, rarenessConfiguration.MaxHitChance);
            var weightConfiguration = GetWeightConfiguration(material);
            var name = GenerateName(material);
            var description = GenerateDescription(rareness, material);
            var bonusesCount = RandomHelper.GetRandomValue(rarenessConfiguration.MinBonuses, rarenessConfiguration.MaxBonuses);
            var maxDurability = weightConfiguration.Durability;

            var item = new WeaponItem
            {
                Name = name,
                Key = Guid.NewGuid().ToString(),
                Description = description,
                Rareness = rareness,
                Weight = weightConfiguration.Weight,
                MaxDamage = maxDamage,
                MinDamage = minDamage,
                Accuracy = hitChance,
                InventoryImage = inventoryImage,
                WorldImage = worldImage,
                EquippedImageLeft = equippedLeftImage,
                EquippedImageRight = equippedRightImage,
                MaxDurability = maxDurability
            };

            _bonusesGenerator.GenerateBonuses(item, bonusesCount);

            var durabilityPercent = RandomHelper.GetRandomValue(MinDurabilityPercent, MaxDurabilityPercent);
            var durability = Math.Min(item.MaxDurability, (int)Math.Round(item.MaxDurability * (durabilityPercent / 100d)));
            item.Durability = durability;

            return item;
        }

        private ISymbolsImage GetMaterialColoredImage(string imageName, ItemMaterial material)
        {
            var image = _imagesStorage.GetImage(imageName);
            return ItemRecolorHelper.RecolorItemImage(image, material);
        }

        private Dictionary<Element, int> GenerateMaxDamage(IWeaponRarenessConfiguration config)
        {
            return config.Damage.ToDictionary(pair => pair.Element,
                pair => RandomHelper.GetRandomValue(pair.MinValue, pair.MaxValue));
        }

        private IWeightConfiguration GetWeightConfiguration(ItemMaterial material)
        {
            var result = _configuration.Weight.FirstOrDefault(config => config.Material == material);
            if (result == null)
                throw new ApplicationException($"No {_baseName} weight configuration for material: {material}");

            return result;
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

        private IWeaponRarenessConfiguration GetRarenessConfiguration(ItemRareness rareness)
        {
            var result = _configuration.RarenessConfiguration.FirstOrDefault(config => config.Rareness == rareness);
            if (result == null)
                throw new ApplicationException($"No {_baseName} rareness configuration for rareness: {rareness}");

            return result;
        }

        private ISymbolsImage GenerateImage(ItemMaterial material)
        {
            var parts = _configuration.Images.Sprites.OrderBy(sprite => sprite.Index)
                .Select(sprite => _imagesStorage.GetImage(RandomHelper.GetRandomElement(sprite.Images)))
                .Select(image => ItemRecolorHelper.RecolorItemImage(image, material))
                .ToArray();
            return MergeImages(parts);
        }

        private string GenerateName(ItemMaterial material)
        {
            var materialPrefix = NameGenerationHelper.GetMaterialPrefix(material);
            return $"{materialPrefix} {_baseName}";
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
            var textConfig = _configurations.DescriptionConfiguration.MaterialDescription.FirstOrDefault(config => config.Material == material);
            if (textConfig == null)
                throw new ApplicationException($"Text configuration not found for weapon material: {material}");

            return RandomHelper.GetRandomElement(textConfig.Text);
        }

        private string GetRarenessDescription(ItemRareness rareness)
        {
            var textConfig = _configurations.DescriptionConfiguration.RarenessDescription.FirstOrDefault(config => config.Rareness == rareness);
            if (textConfig == null)
                throw new ApplicationException($"Text configuration not found for weapon rareness: {rareness}");

            return RandomHelper.GetRandomElement(textConfig.Text);
        }
    }
}