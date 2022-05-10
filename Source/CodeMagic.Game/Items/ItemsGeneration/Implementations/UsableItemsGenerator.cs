using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Implementations.Usable;
using CodeMagic.Game.Items.Usable.Potions;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations
{
    public interface IUsableItemsGenerator
    {
        IItem GenerateUsableItem(ItemRareness rareness);
    }

    public class UsableItemsGenerator : IUsableItemsGenerator
    {
        private readonly Dictionary<UsableItemType, IUsableItemTypeGenerator> _generators;

        public UsableItemsGenerator(
            IImagesStorage imagesStorage,
            IAncientSpellsProvider spellsProvider,
            IPotionDataFactory potionDataFactory)
        {
            _generators = new Dictionary<UsableItemType, IUsableItemTypeGenerator>
            {
                {UsableItemType.Potion, new PotionsGenerator(potionDataFactory)},
                {UsableItemType.Scroll, new ScrollsGenerator(spellsProvider)}
            };
        }

        public IItem GenerateUsableItem(ItemRareness rareness)
        {
            var type = GetRandomItemType();
            if (_generators.ContainsKey(type))
            {
                return _generators[type].Generate(rareness);
            }

            throw new ArgumentException($"Unknown usable item type: {type}");
        }

        private UsableItemType GetRandomItemType()
        {
            var types = Enum.GetValues(typeof(UsableItemType)).OfType<UsableItemType>().ToArray();
            return RandomHelper.GetRandomElement(types);
        }

        private enum UsableItemType
        {
            Potion,
            Scroll
        }
    }
}