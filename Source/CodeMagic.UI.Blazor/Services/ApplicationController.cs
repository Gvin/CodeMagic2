using CodeMagic.UI.Presenters;

namespace CodeMagic.UI.Blazor.Services;

public class ApplicationController : IApplicationController
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ApplicationController> _logger;

    public ApplicationController(IServiceProvider serviceProvider, ILogger<ApplicationController> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public TPresenter CreatePresenter<TPresenter>() where TPresenter : class, IPresenter
    {
        _logger.LogDebug("Creating new presenter of type {PresenterType}", typeof(TPresenter).Name);
        return _serviceProvider.GetRequiredService<TPresenter>();
    }
}
