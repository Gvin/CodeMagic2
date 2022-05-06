using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CodeMagic.UI.Blazor;
using CodeMagic.UI.Blazor.Services;
using CodeMagic.UI;
using CodeMagic.UI.Presenters;
using CodeMagic.UI.Blazor.Models;
using CodeMagic.UI.Services;
using CodeMagic.Game.GameProcess;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Logging
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Services.AddLogging();

// Configuration
builder.Services.AddOptions<SettingsConfiguration>(SettingsConfiguration.ConfigSection);

// Services
builder.Services.AddSingleton<IWindowService, WindowService>();
builder.Services.AddSingleton<IApplicationController, ApplicationController>();
builder.Services.AddSingleton<IApplicationService, ApplicationService>();
builder.Services.AddSingleton<IGameManager>(provider => new GameManager(
    provider.GetRequiredService<ISaveService>(), 
    5,
    provider.GetRequiredService<ILoggerFactory>())); // TODO: Take save interval from settings
builder.Services.AddSingleton<ISettingsService, SettingsService>();
builder.Services.AddSingleton<ISaveService, SaveService>();
builder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();

// Windows
builder.Services.AddTransient<IMainMenuView, MainMenuModel>();
builder.Services.AddTransient<ISettingsView, SettingsModel>();

// Presenters
builder.Services.AddTransient<MainMenuPresenter>();
builder.Services.AddTransient<SettingsPresenter>();

// Starting
var application = builder.Build();

application.Services.GetRequiredService<IApplicationController>().CreatePresenter<MainMenuPresenter>().Run();

await application.RunAsync();
