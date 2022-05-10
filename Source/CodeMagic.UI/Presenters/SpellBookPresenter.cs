using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items;
using CodeMagic.Game.Items.Materials;
using CodeMagic.Game.Items.Usable;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.PlayerActions;
using CodeMagic.Game.Spells;
using CodeMagic.UI.Services;

namespace CodeMagic.UI.Presenters
{
    public interface ISpellBookView : IView
    {
        event EventHandler Exit;
        event EventHandler SaveToLibrary;
        event EventHandler LoadFromLibrary;
        event EventHandler RemoveSpell;
        event EventHandler CastSpell;
        event EventHandler ScribeSpell;
        event EventHandler EditSpell;

        BookSpell SelectedSpell { get; }

        int SelectedSpellIndex { get; }

        ISpellBook SpellBook { set; }

        int? PlayerMana { set; }

        bool CanScribe { set; }

        void RefreshSpells();

        void Initialize();
    }

    public class SpellBookPresenter : IPresenter
    {
        private const string DefaultSpellName = "New Spell";

        private readonly ISpellBookView _view;
        private readonly IApplicationController _controller;
        private readonly ISpellsLibraryService _spellsLibraryService;
        private GameCore<Player> _game;

        public SpellBookPresenter(ISpellBookView view, IApplicationController controller, ISpellsLibraryService spellsLibraryService)
        {
            _view = view;
            _controller = controller;
            _spellsLibraryService = spellsLibraryService;

            _view.Exit += View_Exit;
            _view.SaveToLibrary += View_SaveToLibrary;
            _view.RemoveSpell += View_RemoveSpell;
            _view.CastSpell += View_CastSpell;
            _view.ScribeSpell += View_ScribeSpell;
            _view.EditSpell += View_EditSpell;
            _view.LoadFromLibrary += View_LoadFromLibrary;
        }

        private ISpellBook CurrentSpellBook
        {
            get
            {
                var spellBookId = _game.Player.Equipment.SpellBookId;
                return _game.Player.Inventory.GetItemById<ISpellBook>(spellBookId);
            }
        }

        private void View_LoadFromLibrary(object sender, EventArgs e)
        {
            _controller.CreatePresenter<LoadSpellPresenter>().Run(((result, spell) =>
            {
                if (!result)
                    return;

                CurrentSpellBook.Spells[_view.SelectedSpellIndex] = spell;
                _view.RefreshSpells();
            }));
        }

        private void View_EditSpell(object sender, EventArgs e)
        {
            var editSpellPresenter = _controller.CreatePresenter<EditSpellPresenter>();

            void EditCallback(bool result, BookSpell spell)
            {
                if (!result)
                    return;

                spell.Name = string.IsNullOrEmpty(spell.Name) ? DefaultSpellName : spell.Name;
                CurrentSpellBook.Spells[_view.SelectedSpellIndex] = spell;
                _view.RefreshSpells();
            }

            if (_view.SelectedSpell == null)
            {
                editSpellPresenter.Run(EditCallback);
            }
            else
            {
                editSpellPresenter.Run(_view.SelectedSpell, EditCallback);
            }
        }

        private void View_ScribeSpell(object sender, EventArgs e)
        {
            if (_view.SelectedSpell == null)
                return;

            var blankScroll = _game.Player.Inventory.GetItem(BlankScroll.ItemKey);
            if (blankScroll == null)
                return;

            var scrollCreationCost = _view.SelectedSpell.ManaCost * 2;
            if (_game.Player.Mana < scrollCreationCost)
            {
                _game.Journal.Write(new NotEnoughManaToScrollMessage());
                return;
            }

            _game.Player.Mana -= scrollCreationCost;

            _game.Player.Inventory.RemoveItem(blankScroll);
            var newScroll = new Scroll
            {
                Name = $"{_view.SelectedSpell.Name} Scroll ({_view.SelectedSpell.ManaCost})",
                Key = Guid.NewGuid().ToString(),
                Weight = 1,
                Code = _view.SelectedSpell.Code,
                SpellName = _view.SelectedSpell.Name,
                Mana = _view.SelectedSpell.ManaCost,
                Rareness = ItemRareness.Uncommon
            };
            _game.Player.Inventory.AddItem(newScroll);

            _view.Close();
            _game.PerformPlayerAction(new EmptyPlayerAction());
        }

        private void View_CastSpell(object sender, EventArgs e)
        {
            if (_view.SelectedSpell == null)
                return;

            _view.Close();
            _game.PerformPlayerAction(new CastSpellPlayerAction(_view.SelectedSpell));
        }

        private void View_RemoveSpell(object sender, EventArgs e)
        {
            if (_view.SelectedSpell != null)
                return;

            CurrentSpellBook.Spells[_view.SelectedSpellIndex] = null;
            _view.RefreshSpells();
        }

        private void View_SaveToLibrary(object sender, EventArgs e)
        {
            if (_view.SelectedSpell == null)
                return;

            _spellsLibraryService.SaveSpell(_view.SelectedSpell);
        }

        private void View_Exit(object sender, EventArgs e)
        {
            _view.Close();
        }

        public void Run(GameCore<Player> currentGame)
        {
            _game = currentGame;
            _view.PlayerMana = _game.Player.Mana;
            _view.SpellBook = CurrentSpellBook;
            _view.CanScribe = _game.Player.Inventory.Contains(BlankScroll.ItemKey);

            _view.Initialize();

            _view.Show();
        }
    }
}