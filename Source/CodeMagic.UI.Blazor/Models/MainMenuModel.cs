using CodeMagic.UI.Blazor.Services;
using CodeMagic.UI.Presenters;

namespace CodeMagic.UI.Blazor.Models
{
	public class MainMenuModel : WindowModelBase, IMainMenuView
	{
		public MainMenuModel(IWindowService windowService)
			: base(windowService) 
		{
		}

		public event EventHandler? StartGame;

		public event EventHandler? ContinueGame;

		public event EventHandler? ShowSpellLibrary;

		public event EventHandler? ShowSettings;

		public event EventHandler? Exit;

		public bool ShowContinueButton { get; private set; }

		public void SetContinueOptionState(bool canContinue)
		{
			ShowContinueButton = canContinue;
		}

		public void OnStartGame()
		{
			StartGame?.Invoke(this, EventArgs.Empty);
		}

		public void OnContinueGame()
		{
			ContinueGame?.Invoke(this, EventArgs.Empty);
		}

		public void OnShowSpellLibrary()
		{
			ShowSpellLibrary?.Invoke(this, EventArgs.Empty);
		}

		public void OnShowSettings()
		{
			ShowSettings?.Invoke(this, EventArgs.Empty);
		}

		public void OnExit()
		{
			Exit?.Invoke(this, EventArgs.Empty);
		}
	}
}
