using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.Usable.Potions;

namespace CodeMagic.Game
{
    public interface IGameData
    {
        PotionType GetPotionType(PotionColor potionColor);
    }

    [Serializable]
    public class GameData : IGameData
    {
        public static IGameData Current { get; private set; }

        public static void Initialize(IGameData data)
        {
            Current = data;
        }

        public Dictionary<PotionColor, PotionType> PotionsPattern { get; set; }

        public PotionType GetPotionType(PotionColor potionColor)
        {
            if (PotionsPattern == null)
            {
                PotionsPattern = GeneratePotionTypes();
            }

            return PotionsPattern[potionColor];
        }

        private static Dictionary<PotionColor, PotionType> GeneratePotionTypes()
        {
            var result = new Dictionary<PotionColor, PotionType>();

            var colors = Enum.GetValues(typeof(PotionColor)).Cast<PotionColor>().ToList();
            var types = Enum.GetValues(typeof(PotionType)).Cast<PotionType>().ToList();

            foreach (var potionColor in colors)
            {
                var type = RandomHelper.GetRandomElement(types.ToArray());
                types.Remove(type);
                result.Add(potionColor, type);
            }

            return result;
        }
    }
}