using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;

namespace CodeMagic.Game.PlayerActions
{
    public abstract class PlayerActionBase : IPlayerAction
    {
        public bool Perform(out Point newPosition)
        {
            var game = CurrentGame.Game;
            var result = Perform(game, out newPosition);

            if (result)
            {
                game.Player.Stamina += RestoresStamina;
            }
            return result;
        }

        protected abstract int RestoresStamina { get; }

        protected abstract bool Perform(IGameCore game, out Point newPosition);
    }
}