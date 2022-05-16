using CodeMagic.Core.Game;
using CodeMagic.UI.Blazor.Services;
using CodeMagic.UI.Presenters;

namespace CodeMagic.UI.Blazor.Models;

public class GameViewModel : WindowModelBase, IGameView
{
    public event EventHandler? OpenInGameMenu;
    public event EventHandler? OpenSpellBook;
    public event EventHandler? OpenInventory;
    public event EventHandler? OpenPlayerStats;
    public event EventHandler? OpenGroundView;
    public event EventHandler? OpenCheats;

    public GameViewModel(IWindowService windowService) : base(windowService)
    {
        SpellBookEnabled = false;
        OpenGroundEnabled = false;
    }

    public bool SpellBookEnabled { get; set; }

    public bool OpenGroundEnabled { get; set; }

    public IGameCore? Game { get; set; }

    public void Initialize()
    {
        // TODO: Implement
    }
}