using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.UI.Blazor.Exceptions;

namespace CodeMagic.UI.Blazor.Services;

public class ConfigurationProviderService
{
    private IItemGeneratorConfiguration? _itemGeneratorConfiguration;

    public async Task Initialize()
    {
        // TODO: Load
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