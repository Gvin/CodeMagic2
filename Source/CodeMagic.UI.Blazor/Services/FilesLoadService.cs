using Newtonsoft.Json;

namespace CodeMagic.UI.Blazor.Services;

public interface IFilesLoadService
{
    Task<string> LoadFileAsStringAsync(string filePath);

    Task<T?> LoadFileAsync<T>(string filePath, JsonSerializerSettings? serializerSettings = null);
}

public class FilesLoadService : IFilesLoadService
{
    private readonly HttpClient _httpClient;

    public FilesLoadService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<string> LoadFileAsStringAsync(string filePath)
    {
        return _httpClient.GetStringAsync(filePath);
    }

    public async Task<T?> LoadFileAsync<T>(string filePath, JsonSerializerSettings? serializerSettings = null)
    {
        var stringData = await _httpClient.GetStringAsync(filePath);
        if (string.IsNullOrEmpty(stringData))
        {
            return default;
        }

        return JsonConvert.DeserializeObject<T>(stringData, serializerSettings);
    }
}