using System;
using CodeMagic.Core.Game;
using CodeMagic.Game.GameProcess;

namespace CodeMagic.UI.Presenters;

public interface IInGameMenuView : IView
{
    event EventHandler ContinueGame;
    event EventHandler StartNewGame;
    event EventHandler ExitToMenu;
}

public class InGameMenuPresenter : IPresenter
{
    private readonly IInGameMenuView _view;
    private readonly IApplicationController _controller;
    private readonly IGameManager _gameManager;
    private IGameCore _currentGame;

    public InGameMenuPresenter(
        IInGameMenuView view, 
        IApplicationController controller, 
        IGameManager gameManager)
    {
        _view = view;
        _controller = controller;
        _gameManager = gameManager;

        _view.ExitToMenu += View_ExitToMenu;
        _view.StartNewGame += View_StartNewGame;
        _view.ContinueGame += View_ContinueGame;
    }

    private void View_ContinueGame(object sender, EventArgs e)
    {
        _view.Close();

        _controller.CreatePresenter<GameViewPresenter>().Run(_currentGame);
    }

    private void View_StartNewGame(object sender, EventArgs e)
    {
        _currentGame.Dispose();

        _view.Close();

        var waitMessagePresenter = _controller.CreatePresenter<WaitMessagePresenter>();
        waitMessagePresenter.Run("Starting new game...", () =>
        {
            var game = _gameManager.StartGame();
            _controller.CreatePresenter<GameViewPresenter>().Run(game);
        });
    }

    private void View_ExitToMenu(object sender, EventArgs e)
    {
        _view.Close();

        _controller.CreatePresenter<MainMenuPresenter>().Run();
    }

    public void Run(IGameCore game)
    {
        _currentGame = game;

        _view.Show();
    }
}