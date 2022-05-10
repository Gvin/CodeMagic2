using System;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Game.Objects;

namespace CodeMagic.Game.Spells
{
    [Serializable]
    public class BookSpell
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public int ManaCost { get; set; }

        public CodeSpell CreateCodeSpell(ICreatureObject caster)
        {
            return new CodeSpell(caster, Code)
            {
                Name = Name,
                Mana = ManaCost
            };
        }
    }
}