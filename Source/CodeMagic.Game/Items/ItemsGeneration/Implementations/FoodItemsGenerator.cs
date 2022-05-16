using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.Usable.Food;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations
{
    public class FoodItemsGenerator
    {
        private static readonly Func<IImagesStorageService, IItem>[] Generators = {
            CreateApple,
            CreateMeat
        };

        private readonly IImagesStorageService _imagesStorage;

        public FoodItemsGenerator(IImagesStorageService imagesStorage)
        {
            _imagesStorage = imagesStorage;
        }

        public IItem GenerateFood()
        {
            var generator = RandomHelper.GetRandomElement(Generators);
            return generator(_imagesStorage);
        }

        private static IItem CreateMeat(IImagesStorageService storage)
        {
            return new FoodItem
            {
                Key = "food_meat",
                Name = "Meat",
                HungerDecrease = 7,
                Rareness = ItemRareness.Common,
                Weight = 500,
                InventoryImageName = "Food_Meat",
                WorldImageName = "ItemsOnGround_Food_Meat",
                Description = new[]
                {
                    "A big piece of meat. It smells good."
                }
            };
        }

        private static IItem CreateApple(IImagesStorageService storage)
        {
            return new FoodItem
            {
                Key = "food_apple",
                Name = "Apple",
                HungerDecrease = 3,
                Rareness = ItemRareness.Common,
                Weight = 200,
                InventoryImageName = "Food_Apple",
                WorldImageName = "ItemsOnGround_Food_Apple",
                Description = new []
                {
                    "A sweet red apple. Juicy and tasty."
                }
            };
        }
    }
}