using Microsoft.JSInterop;

namespace CodeMagic.UI.Blazor.Services;

public interface IDownloadFileService
{
    Task DownloadAsync(string fileName, string content);
}

public class DownloadFileService : IDownloadFileService
{
    private readonly IJSRuntime _jsRuntime;

    public DownloadFileService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task DownloadAsync(string fileName, string content)
    {
        await _jsRuntime.InvokeVoidAsync("saveFile", fileName, content);
    }
}