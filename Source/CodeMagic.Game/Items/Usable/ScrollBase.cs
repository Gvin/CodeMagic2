using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Images;
using CodeMagic.Game.Objects;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items.Usable;

public interface IScroll : IUsableItem
{
    string SpellName { get; }

    int Mana { get; }
}

public abstract class ScrollBase : Item, IScroll, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
{
    public string SpellName { get; set; }

    public string Code { get; set; }

    public int Mana { get; set; }

    public virtual bool Use(IGameCore game)
    {
        var codeSpell = new CodeSpell(game.Player, Code) {Name = SpellName, Mana = Mana};
        game.Map.AddObject(game.PlayerPosition, codeSpell);
        return false;
    }

    public virtual string GetSpellDisplayCode()
    {
        return Code;
    }

    public abstract ISymbolsImage GetWorldImage(IImagesStorageService storage);

    public abstract ISymbolsImage GetInventoryImage(IImagesStorageService storage);

    public abstract StyledLine[] GetDescription(Player player);
}