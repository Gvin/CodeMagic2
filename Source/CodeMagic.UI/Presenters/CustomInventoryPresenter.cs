using System;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.PlayerActions;

namespace CodeMagic.UI.Presenters
{
    public interface ICustomInventoryView : IView
    {
        event EventHandler PickUpOne;
        event EventHandler PickUpStack;
        event EventHandler PickUpAll;
        event EventHandler Exit;

        string InventoryName { set; }

        IInventoryStack SelectedStack { get; }

        IInventoryStack[] Stacks { set; }

        Player Player { set; }

        void Initialize();

        void RefreshItems(bool keepSelection);
    }

    public class CustomInventoryPresenter : IPresenter
    {
        private readonly ICustomInventoryView _view;
        private GameCore<Player> _game;
        private IInventory _inventory;
        private bool _actionPerformed;

        public CustomInventoryPresenter(ICustomInventoryView view)
        {
            _view = view;
            _actionPerformed = false;

            _view.Exit += View_Exit;
            _view.PickUpOne += View_PickUpOne;
            _view.PickUpStack += View_PickUpStack;
            _view.PickUpAll += View_PickUpAll;
        }

        private void View_PickUpAll(object sender, EventArgs e)
        {
            _actionPerformed = true;
            foreach (var stack in _inventory.Stacks)
            {
                var items = stack.Items.ToArray();
                foreach (var item in items)
                {
                    _inventory.RemoveItem(item);
                    _game.Player.Inventory.AddItem(item);
                }
            }
            CloseView();
        }

        private void View_PickUpStack(object sender, EventArgs e)
        {
            if (_view.SelectedStack == null)
                return;

            _actionPerformed = true;
            var items = _view.SelectedStack.Items.ToArray();
            foreach (var item in items)
            {
                _inventory.RemoveItem(item);
                _game.Player.Inventory.AddItem(item);
            }

            if (_inventory.ItemsCount > 0)
            {
                _view.Stacks = _inventory.Stacks;
                _view.RefreshItems(false);
            }
            else
            {
                CloseView();
            }
        }

        private void View_PickUpOne(object sender, EventArgs e)
        {
            if (_view.SelectedStack == null)
                return;

            _actionPerformed = true;
            var item = _view.SelectedStack.TopItem;
            _inventory.RemoveItem(item);
            _game.Player.Inventory.AddItem(item);

            if (_inventory.ItemsCount > 0)
            {
                _view.Stacks = _inventory.Stacks;
                _view.RefreshItems(true);
            }
            else
            {
                CloseView();
            }
        }

        private void CloseView()
        {
            if (_actionPerformed)
            {
                _game.PerformPlayerAction(new EmptyPlayerAction());
            }
            _view.Close();
        }

        private void View_Exit(object sender, EventArgs e)
        {
            CloseView();
        }

        public void Run(GameCore<Player> currentGame, string inventoryName, IInventory customInventory)
        {
            _game = currentGame;
            _inventory = customInventory;
            _view.InventoryName = inventoryName;
            _view.Stacks = _inventory.Stacks;
            _view.Player = _game.Player;

            _view.Initialize();
            _view.Show();
        }
    }
}