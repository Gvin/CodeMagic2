using CodeMagic.Core.Common;
using CodeMagic.UI.Blazor.Models;

namespace CodeMagic.UI.Blazor.Services
{
	public interface IWindowService
	{
		event EventHandler WindowsChanged;

		void Open(IWindowModel window);

		void Close(IWindowModel window);

		IWindowModel? GetCurrentWindow();
	}

	public class WindowService : IWindowService
	{
		private readonly ConcurrentList<IWindowModel> _windows;
		private readonly ILogger<WindowService> _logger;

		public event EventHandler? WindowsChanged;

		public WindowService(ILogger<WindowService> logger)
        {
            _logger = logger;
            _windows = new ConcurrentList<IWindowModel>();
        }

		public void Open(IWindowModel window)
		{
			_logger.LogDebug("Opening window of type {WindowType}", window.GetType().Name);
			_windows.Add(window);
			OnWindowsChanged();
		}

		public void Close(IWindowModel window)
		{
			_logger.LogDebug("Closing window of type {WindowType}", window.GetType().Name);

			var index = _windows.IndexOf(window);
			if (index == -1)
			{
				throw new ArgumentException($"Window of type {window.GetType().Name} not found");
			}

			_windows.RemoveAt(index);
			OnWindowsChanged();
		}

		public IWindowModel? GetCurrentWindow()
		{
			return _windows.LastOrDefault();
		}

		private void OnWindowsChanged()
		{
			WindowsChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
