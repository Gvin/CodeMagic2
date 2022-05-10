using System;
using CodeMagic.Core.Objects;

namespace CodeMagic.UI.Presenters
{
    public interface IPlayerStatsView : IView
    {
        event EventHandler Exit;

        IPlayer Player { set; }
    }

    public class PlayerStatsPresenter : IPresenter
    {
        private readonly IPlayerStatsView view;

        public PlayerStatsPresenter(IPlayerStatsView view)
        {
            this.view = view;

            this.view.Exit += View_Exit;
        }

        private void View_Exit(object sender, EventArgs e)
        {
            view.Close();
        }

        public void Run(IPlayer player)
        {
            view.Player = player;

            view.Show();
        }
    }
}