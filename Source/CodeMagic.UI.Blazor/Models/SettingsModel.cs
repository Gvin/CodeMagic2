using CodeMagic.UI.Blazor.Services;
using CodeMagic.UI.Presenters;

namespace CodeMagic.UI.Blazor.Models
{
	public class SettingsModel : WindowModelBase, ISettingsView
	{
		public event EventHandler? IncreaseFontSize;

		public event EventHandler? DecreaseFontSize;

		public event EventHandler? Exit;

        public event EventHandler? IncreaseBrightness;

        public event EventHandler? DecreaseBrightness;

        public event EventHandler? IncreaseSavingInterval;

        public event EventHandler? DecreaseSavingInterval;

        public SettingsModel(IWindowService windowService)
			: base(windowService)
		{
		}

		public string? FontSizeName { get; set; }

        public float Brightness { get; set; }

		public int SavingInterval { get; set; }

		public void OnIncreaseFontSize()
        {
			IncreaseFontSize?.Invoke(this, EventArgs.Empty);
        }

		public void OnDecreaseFontSize()
		{
			DecreaseFontSize?.Invoke(this, EventArgs.Empty);
		}

		public void OnIncreaseBrightness()
		{
			IncreaseBrightness?.Invoke(this, EventArgs.Empty);
		}

		public void OnDecreaseBrightness()
		{
			DecreaseBrightness?.Invoke(this, EventArgs.Empty);
		}

		public void OnIncreaseSavingInterval()
		{
			IncreaseSavingInterval?.Invoke(this, EventArgs.Empty);
		}

		public void OnDecreaseSavingInterval()
		{
			DecreaseSavingInterval?.Invoke(this, EventArgs.Empty);
		}

		public void OnExit()
		{
			Exit?.Invoke(this, EventArgs.Empty);
		}
	}
}
