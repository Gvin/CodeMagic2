using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;

namespace CodeMagic.Game
{
    public static class AttackHelper
    {
        public static int CalculateDamage(int damage, Element element, IPlayer player)
        {
            if (element == Element.Piercing ||
                element == Element.Slashing ||
                element == Element.Blunt)
            {
                return (int) Math.Round(damage * (1d + player.DamageBonus / 100d));
            }

            return damage;
        }
    }
}