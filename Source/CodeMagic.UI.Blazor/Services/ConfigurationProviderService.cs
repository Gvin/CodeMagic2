using CodeMagic.Configuration.Json;
using CodeMagic.Configuration.Json.Exceptions;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.UI.Blazor.Exceptions;

namespace CodeMagic.UI.Blazor.Services;

public class ConfigurationProviderService
{
    private const string ItemGeneratorConfigurationPath = "./resources/configuration/item-generator.json";

    private readonly IFilesLoadService _filesLoadService;
    private readonly IJsonConfigurationLoader _configurationLoader;
    private readonly ILogger<ConfigurationProviderService> _logger;

    private IItemGeneratorConfiguration? _itemGeneratorConfiguration;

    public ConfigurationProviderService(
        IFilesLoadService filesLoadService,
        IJsonConfigurationLoader configurationLoader,
        ILogger<ConfigurationProviderService> logger)
    {
        _filesLoadService = filesLoadService;
        _logger = logger;
        _configurationLoader = configurationLoader;
    }

    public async Task Initialize()
    {
        _logger.LogDebug("Loading configuration");

        _itemGeneratorConfiguration = await LoadConfigurationFile(ItemGeneratorConfigurationPath,
            _configurationLoader.LoadItemGeneratorConfiguration);

        _logger.LogDebug("Configuration loaded");
    }

    private async Task<T> LoadConfigurationFile<T>(string fileName, Func<Stream, T> loader)
    {
        _logger.LogDebug("Loading configuration {Configuration}", typeof(T).Name);

        try
        {
            await using var stream = await _filesLoadService.OpenFileStream(fileName);

            var result = loader(stream);

            if (result == null)
            {
                throw new ApplicationLoadException($"Unable to load configuration {typeof(T).Name}");
            }

            return result;
        }
        catch (JsonSchemaValidationException ex)
        {
            _logger.LogCritical(
                ex,
                "Found {ErrorsCount} error when validating configuration schema for {Configuration}: {@Errors}",
                ex.Errors.Length,
                typeof(T).Name,
                ex.Errors);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error while loading configuration {Configuration}", typeof(T).Name);
            throw;
        }
    }

    public IItemGeneratorConfiguration GetItemGeneratorConfiguration()
    {
        if (_itemGeneratorConfiguration == null)
        {
            throw new ApplicationLoadException($"{nameof(ConfigurationProviderService)} was not initialized.");
        }

        return _itemGeneratorConfiguration;
    }
}