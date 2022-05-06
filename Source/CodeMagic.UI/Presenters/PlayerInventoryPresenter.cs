using System;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items;
using CodeMagic.Game.Items.Usable;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.PlayerActions;
using CodeMagic.UI.Services;

namespace CodeMagic.UI.Presenters
{
    public interface IPlayerInventoryView : IView
    {
        event EventHandler Exit;
        event EventHandler UseItem;
        event EventHandler EquipItem;
        event EventHandler EquipHoldableItemLeft;
        event EventHandler EquipHoldableItemRight;
        event EventHandler DropItem;
        event EventHandler DropStack;
        event EventHandler TakeOffItem;
        event EventHandler CheckScroll;

        Player Player { set; }

        IInventoryStack[] Stacks { set; }

        IInventoryStack SelectedStack { get; }

        void Initialize();
    }

    public class PlayerInventoryPresenter : IPresenter
    {
        private readonly IPlayerInventoryView _view;
        private readonly IEditSpellService _editSpellService;
        private GameCore<Player> _game;

        public PlayerInventoryPresenter(IPlayerInventoryView view, IEditSpellService editSpellService)
        {
            this._view = view;
            this._editSpellService = editSpellService;

            this._view.Exit += View_Exit;
            this._view.UseItem += View_UseItem;
            this._view.EquipItem += View_EquipItem;
            this._view.EquipHoldableItemLeft += View_EquipHoldableItemLeft;
            this._view.EquipHoldableItemRight += View_EquipHoldableItemRight;
            this._view.DropItem += View_DropItem;
            this._view.DropStack += View_DropStack;
            this._view.TakeOffItem += View_TakeOffItem;
            this._view.CheckScroll += View_CheckScroll;
        }

        private void View_CheckScroll(object sender, EventArgs e)
        {
            if (!(_view.SelectedStack?.TopItem is ScrollBase selectedScroll))
                return;

            var code = selectedScroll.GetSpellDisplayCode();
            var filePath = _editSpellService.PrepareSpellTemplate(code);
            _editSpellService.LaunchSpellFileEditor(filePath);
        }

        private void View_TakeOffItem(object sender, EventArgs e)
        {
            if (!(_view.SelectedStack?.TopItem is IEquipableItem equipableItem))
                return;

            if (!_game.Player.Equipment.IsEquiped(equipableItem))
                return;

            _game.PerformPlayerAction(new UnequipItemPlayerAction(equipableItem));
            _view.Close();
        }

        private void View_DropStack(object sender, EventArgs e)
        {
            if (_view.SelectedStack == null)
                return;

            _game.PerformPlayerAction(new DropItemsPlayerAction(_view.SelectedStack.Items));
            _view.Close();
        }

        private void View_DropItem(object sender, EventArgs e)
        {
            if (_view.SelectedStack == null)
                return;

            _game.PerformPlayerAction(new DropItemsPlayerAction(_view.SelectedStack.TopItem));
            _view.Close();
        }

        private void View_EquipHoldableItemRight(object sender, EventArgs e)
        {
            EquipSelectedHoldable(true);
        }

        private void View_EquipHoldableItemLeft(object sender, EventArgs e)
        {
            EquipSelectedHoldable(false);
        }

        private void EquipSelectedHoldable(bool isRight)
        {
            if (!(_view.SelectedStack?.TopItem is IHoldableItem holdableItem))
                return;

            if (_game.Player.Equipment.IsEquiped(holdableItem))
                return;

            _game.PerformPlayerAction(new EquipHoldablePlayerAction(holdableItem, isRight));
            _view.Close();
        }

        private void View_EquipItem(object sender, EventArgs e)
        {
            if (!(_view.SelectedStack?.TopItem is IEquipableItem equipableItem))
                return;

            if (_game.Player.Equipment.IsEquiped(equipableItem))
                return;

            if (equipableItem is IHoldableItem)
                return;

            _game.PerformPlayerAction(new EquipItemPlayerAction(equipableItem));
            _view.Close();
        }

        private void View_UseItem(object sender, EventArgs e)
        {
            if (!(_view.SelectedStack?.TopItem is IUsableItem usableItem))
                return;

            _game.PerformPlayerAction(new UseItemPlayerAction(usableItem));
            _view.Close();
        }

        private void View_Exit(object sender, EventArgs e)
        {
            _view.Close();
        }

        public void Run(GameCore<Player> currentGame)
        {
            _game = currentGame;
            _view.Player = _game.Player;
            _view.Stacks = _game.Player.Inventory.Stacks.OrderByDescending(stack => GetIfEquipped(_game.Player, stack)).ToArray();

            _view.Initialize();
            _view.Show();
        }

        private static bool GetIfEquipped(Player player, IInventoryStack stack)
        {
            if (!(stack.TopItem is IEquipableItem equipable))
                return false;

            return player.Equipment.IsEquiped(equipable);
        }
    }
}