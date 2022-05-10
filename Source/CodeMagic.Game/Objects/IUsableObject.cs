using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Objects
{
    public interface IUsableObject
    {
        bool CanUse { get; }

        void Use(IGameCore game, Point position);
    }
}