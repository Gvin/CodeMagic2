using CodeMagic.UI.Services;
using Microsoft.JSInterop;

namespace CodeMagic.UI.Blazor.Services
{
	public class ApplicationService : IApplicationService
	{
		private readonly IJSRuntime _jsRuntime;

		public ApplicationService(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
		}

		public void Exit()
		{
			_jsRuntime.InvokeVoidAsync("window.close").AsTask().Wait();
		}
	}
}
