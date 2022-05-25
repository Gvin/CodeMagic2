using CodeMagic.UI.Services;
using Microsoft.Extensions.Options;

namespace CodeMagic.UI.Blazor.Services;

public class SettingsConfiguration
{
    public float Brightness { get; set; }

    public bool DebugDrawTemperature { get; set; }

    public bool DebugDrawLightLevel { get; set; }

    public bool DebugDrawMagicEnergy { get; set; }

    public FontSizeMultiplier FontSize { get; set; }

    public int MinActionsInterval { get; set; }

    public int SavingInterval { get; set; }

    public bool DebugWriteMapToFile { get; set; }
}

public class SettingsService : ISettingsService
{
    private readonly ILocalStorageService _localStorage;
    private readonly IOptions<SettingsConfiguration> _config;
    private readonly ILogger<SettingsService> _logger;

    public SettingsService(
        ILocalStorageService localStorage,
        IOptions<SettingsConfiguration> config,
        ILogger<SettingsService> logger)
    {
        _localStorage = localStorage;
        _config = config;
        _logger = logger;
    }

    public float Brightness { get; set; }

    public bool DebugDrawTemperature { get; set; }

    public bool DebugDrawLightLevel { get; set; }

    public bool DebugDrawMagicEnergy { get; set; }

    public FontSizeMultiplier FontSize { get; set; }

    public int SavingInterval { get; set; }

    public int MinActionsInterval => _config.Value.MinActionsInterval;

    public bool DebugWriteMapToFile => _config.Value.DebugWriteMapToFile;

    public async void Save()
    {
        _logger.LogDebug("Saving settings.");

        var model = new SettingsLocalStorage
        {
            Brightness = Brightness,
            DebugDrawTemperature = DebugDrawTemperature,
            DebugDrawLightLevel = DebugDrawLightLevel,
            DebugDrawMagicEnergy = DebugDrawMagicEnergy,
            FontSize = FontSize,
            SavingInterval = SavingInterval
        };

        await _localStorage.SetAsync(SettingsLocalStorage.Key, model);
    }

    public async Task LoadAsync()
    {
        _logger.LogDebug("Loading settings.");

        var storedSettings = await _localStorage.GetAsync<SettingsLocalStorage>(SettingsLocalStorage.Key);

        if (storedSettings == null)
        {
            _logger.LogDebug("No stored settings found. Using default values.");
        }

        Brightness = storedSettings?.Brightness ?? _config.Value.Brightness;
        DebugDrawTemperature = storedSettings?.DebugDrawTemperature ?? _config.Value.DebugDrawTemperature;
        DebugDrawMagicEnergy = storedSettings?.DebugDrawMagicEnergy ?? _config.Value.DebugDrawMagicEnergy;
        DebugDrawLightLevel = storedSettings?.DebugDrawLightLevel ?? _config.Value.DebugDrawLightLevel;
        FontSize = storedSettings?.FontSize ?? _config.Value.FontSize;
        SavingInterval = storedSettings?.SavingInterval ?? _config.Value.SavingInterval;
    }

    public class SettingsLocalStorage
    {
        public const string Key = "Settings";

        public float Brightness { get; set; }

        public bool DebugDrawTemperature { get; set; }

        public bool DebugDrawLightLevel { get; set; }

        public bool DebugDrawMagicEnergy { get; set; }

        public FontSizeMultiplier FontSize { get; set; }

        public int SavingInterval { get; set; }
    }
}