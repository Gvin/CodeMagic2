using CodeMagic.UI.Blazor.Services;

namespace CodeMagic.UI.Blazor.Models
{
	public abstract class WindowModelBase : IWindowModel
	{
		private readonly IWindowService _windowService;

		protected WindowModelBase(IWindowService windowService)
		{
			_windowService = windowService;
		}

		public void Show()
		{
			_windowService.Open(this);
		}

		public void Close()
		{
			_windowService.Close(this);
		}
	}
}
