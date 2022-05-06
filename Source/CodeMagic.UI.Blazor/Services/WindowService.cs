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

		public event EventHandler? WindowsChanged;

		public WindowService()
		{
			_windows = new ConcurrentList<IWindowModel>();
		}

		public void Open(IWindowModel window)
		{
			_windows.Add(window);
			OnWindowsChanged();
		}

		public void Close(IWindowModel window)
		{
			var index = _windows.IndexOf(window);
			if (index == -1)
			{
				throw new ArgumentException($"Window of type {nameof(IWindowModel)} not found");
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
