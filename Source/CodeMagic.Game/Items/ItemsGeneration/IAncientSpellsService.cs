using CodeMagic.Game.Spells;

namespace CodeMagic.Game.Items.ItemsGeneration
{
    public interface IAncientSpellsService
    {
        BookSpell[] GetUncommonSpells();

        BookSpell[] GetRareSpells();
    }
}