using System.Net;
using Newtonsoft.Json;

namespace CodeMagic.UI.Blazor.Services;

public interface IFilesLoadService
{
    Task<string> LoadFileAsStringAsync(string filePath);

    Task<T?> LoadFileAsync<T>(string filePath, JsonSerializerSettings? serializerSettings = null);

    Task<Stream> OpenFileStream(string filePath);
}

public class FilesLoadService : IFilesLoadService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<FilesLoadService> _logger;

    public FilesLoadService(HttpClient httpClient, ILogger<FilesLoadService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string> LoadFileAsStringAsync(string filePath)
    {
        _logger.LogDebug("Loading file path as string: {FilePath}", filePath);

        try
        {
            return await _httpClient.GetStringAsync(filePath);
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new FileNotFoundException("File not found", filePath);
            }

            _logger.LogError(ex, "Error while loading file path: {FilePath}", filePath);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while loading file path: {FilePath}", filePath);
            throw;
        }
    }

    public Task<Stream> OpenFileStream(string filePath)
    {
        _logger.LogDebug("Opening file stream: {FilePath}", filePath);

        try
        {
            return _httpClient.GetStreamAsync(filePath);
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new FileNotFoundException("File not found", filePath);
            }

            _logger.LogError(ex, "Error while loading file path: {FilePath}", filePath);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while opening file stream: {FilePath}", filePath);
            throw;
        }
    }

    public async Task<T?> LoadFileAsync<T>(string filePath, JsonSerializerSettings? serializerSettings = null)
    {
        _logger.LogDebug("Loading file path: {FilePath}", filePath);

        try
        {
            var stringData = await _httpClient.GetStringAsync(filePath);
            if (string.IsNullOrEmpty(stringData))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(stringData, serializerSettings);
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new FileNotFoundException("File not found", filePath);
            }

            _logger.LogError(ex, "Error while loading file path: {FilePath}", filePath);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while loading file path: {FilePath}", filePath);
            throw;
        }
    }
}