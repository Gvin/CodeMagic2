using System;
using CodeMagic.Core.Game;
using CodeMagic.Game.GameProcess;

namespace CodeMagic.UI.Presenters
{
    public interface IMainMenuView : IView
    {
        event EventHandler StartGame;

        event EventHandler ContinueGame;

        event EventHandler ShowSpellLibrary;

        event EventHandler ShowSettings;

        void SetContinueOptionState(bool canContinue);
    }

    public class MainMenuPresenter : IPresenter
    {
        private readonly IMainMenuView _view;
        private readonly IApplicationController _controller;
        private readonly IGameManager _gameManager;

        public MainMenuPresenter(
            IMainMenuView view, 
            IApplicationController controller,
            IGameManager gameManager)
        {
            _view = view;
            _controller = controller;
            _gameManager = gameManager;

            _view.SetContinueOptionState(CurrentGame.Game != null);

            _view.StartGame += View_StartGame;
            _view.ContinueGame += View_ContinueGame;
            _view.ShowSpellLibrary += View_ShowSpellLibrary;
            _view.ShowSettings += View_ShowSettings;
        }

        private void View_ShowSettings(object sender, EventArgs e)
        {
            _controller.CreatePresenter<SettingsPresenter>().Run();
        }

        private void View_ShowSpellLibrary(object sender, EventArgs e)
        {
            _controller.CreatePresenter<MainSpellsLibraryPresenter>().Run();
        }

        private void View_ContinueGame(object sender, EventArgs e)
        {
            if (CurrentGame.Game == null)
                return;

            _view.Close();

            _controller.CreatePresenter<GameViewPresenter>().Run(CurrentGame.Game);
        }

        private void View_StartGame(object sender, EventArgs args)
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