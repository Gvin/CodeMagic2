using CodeMagic.Configuration.Json;
using CodeMagic.Configuration.Json.Exceptions;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Configuration.Levels;
using CodeMagic.Game.Configuration.Liquids;
using CodeMagic.Game.Configuration.Monsters;
using CodeMagic.Game.Configuration.Physics;
using CodeMagic.Game.Configuration.Spells;
using CodeMagic.Game.Configuration.Treasure;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.UI.Blazor.Exceptions;

namespace CodeMagic.UI.Blazor.Services;

public class GameConfigurationProviderService : IGameConfigurationProvider
{
    private const string ItemGeneratorConfigurationPath = "./resources/configuration/item-generator.json";
    private const string PhysicsConfigurationPath = "./resources/configuration/physics.json";
    private const string LiquidsConfigurationPath = "./resources/configuration/liquids.json";
    private const string SpellsConfigurationPath = "./resources/configuration/spells.json";
    private const string MonstersConfigurationPath = "./resources/configuration/monsters.json";

    private readonly IFilesLoadService _filesLoadService;
    private readonly IConfigurationLoader _configurationLoader;
    private readonly ILogger<GameConfigurationProviderService> _logger;

    private IItemGeneratorConfiguration? _itemGeneratorConfiguration;
    private IPhysicsConfiguration? _physicsConfiguration;
    private ILiquidsConfiguration? _liquidsConfiguration;
    private ISpellsConfiguration? _spellsConfiguration;
    private IMonstersConfiguration? _monstersConfiguration;

    public GameConfigurationProviderService(
        IFilesLoadService filesLoadService,
        IConfigurationLoader configurationLoader,
        ILogger<GameConfigurationProviderService> logger)
    {
        _filesLoadService = filesLoadService;
        _logger = logger;
        _configurationLoader = configurationLoader;

        var staticConfigurationLoader = new StaticConfigurationLoader();
        Levels = staticConfigurationLoader.GetLevelsConfiguration();
        Treasure = staticConfigurationLoader.GeTreasureConfiguration();
    }

    public async Task Load()
    {
        _logger.LogDebug("Loading configuration");

        _itemGeneratorConfiguration = await LoadConfigurationFile(
            ItemGeneratorConfigurationPath,
            _configurationLoader.LoadItemGeneratorConfiguration);

        _physicsConfiguration = await LoadConfigurationFile(
            PhysicsConfigurationPath,
            _configurationLoader.LoadPhysicsConfiguration);

        _liquidsConfiguration = await LoadConfigurationFile(
            LiquidsConfigurationPath,
            _configurationLoader.LoadLiquidsConfiguration);

        _spellsConfiguration = await LoadConfigurationFile(
            SpellsConfigurationPath,
            _configurationLoader.LoadSpellsConfiguration);

        _monstersConfiguration = await LoadConfigurationFile(
            MonstersConfigurationPath,
            _configurationLoader.LoadMonstersConfiguration);

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
            throw new ApplicationLoadException($"{nameof(GameConfigurationProviderService)} was not initialized.");
        }

        return _itemGeneratorConfiguration;
    }

    public IPhysicsConfiguration Physics => _physicsConfiguration ?? throw new ApplicationLoadException($"{nameof(GameConfigurationProviderService)} was not initialized.");

    public ILiquidsConfiguration Liquids => _liquidsConfiguration ?? throw new ApplicationLoadException($"{nameof(GameConfigurationProviderService)} was not initialized.");

    public ISpellsConfiguration Spells => _spellsConfiguration ?? throw new ApplicationLoadException($"{nameof(GameConfigurationProviderService)} was not initialized.");

    public IMonstersConfiguration Monsters => _monstersConfiguration ?? throw new ApplicationLoadException($"{nameof(GameConfigurationProviderService)} was not initialized.");

    public ILevelsConfiguration Levels { get; }

    public ITreasureConfiguration Treasure { get; }
}