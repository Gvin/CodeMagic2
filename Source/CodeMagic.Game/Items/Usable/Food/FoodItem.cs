using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Usable.Food
{
    public interface IFoodItem : IUsableItem
    {
    }

    public class FoodItem : Item, IFoodItem, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
    {
        public int HungerDecrease { get; set; }

        public string[] Description { get; set; }

        public string InventoryImageName { get; set; }

        public string WorldImageName { get; set; }

        public override bool Stackable => true;

        public bool Use(IGameCore game)
        {
            game.Player.HungerPercent -= HungerDecrease;
            game.Journal.Write(new HungerDecreasedMessage(HungerDecrease));
            return false;
        }

        public ISymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(WorldImageName);
        }

        public ISymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage(InventoryImageName);
        }

        public StyledLine[] GetDescription(Player player)
        {
            var result = new List<StyledLine>
            {
                TextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine {"Hunger Decrease: ", TextHelper.GetValueString(HungerDecrease, "%", false)},
                StyledLine.Empty
            };
            result.AddRange(TextHelper.ConvertDescription(Description));
            return result.ToArray();
        }
    }
}