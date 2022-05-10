using System;
using CodeMagic.Game.GameProcess;

namespace CodeMagic.UI.Presenters
{
    public interface IPlayerDeathView : IView
    {
        event EventHandler StartNewGame;
        event EventHandler ExitToMenu;
    }

    public class PlayerDeathPresenter : IPresenter
    {
        private readonly IPlayerDeathView _view;
        private readonly IApplicationController _controller;
        private readonly IGameManager _gameManager;

        public PlayerDeathPresenter(
            IPlayerDeathView view, 
            IApplicationController controller,
            IGameManager gameManager)
        {
            _view = view;
            _controller = controller;
            _gameManager = gameManager;

            _view.StartNewGame += View_StartNewGame;
            _view.ExitToMenu += View_ExitToMenu;
        }

        private void View_ExitToMenu(object sender, EventArgs e)
        {
            _view.Close();

            _controller.CreatePresenter<MainMenuPresenter>().Run();
        }

        private void View_StartNewGame(object sender, EventArgs e)
        {
            _view.Close();
            
            _controller.CreatePresenter<WaitMessagePresenter>().Run("Starting new game...", () =>
            {
                var game = _gameManager.StartGame();
                _controller.CreatePresenter<GameViewPresenter>().Run(game);
            });
        }

        public void Run()
        {
            _view.Show();
        }
    }
}