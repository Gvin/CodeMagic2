using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Objects;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Usable
{
    public abstract class ScrollBase : Item, IUsableItem, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
    {
        protected ScrollBase()
        {
        }

        public string SpellName { get; set; }

        public string Code { get; set; }

        public int Mana { get; set; }

        public virtual bool Use(GameCore<Player> game)
        {
            var codeSpell = new CodeSpell(game.Player, SpellName, Code, Mana);
            game.Map.AddObject(game.PlayerPosition, codeSpell);
            return false;
        }

        public virtual string GetSpellDisplayCode()
        {
            return Code;
        }

        public abstract SymbolsImage GetWorldImage(IImagesStorage storage);

        public abstract SymbolsImage GetInventoryImage(IImagesStorage storage);

        public abstract StyledLine[] GetDescription(Player player);
    }
}