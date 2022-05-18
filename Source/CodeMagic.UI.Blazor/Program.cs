using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CodeMagic.UI.Blazor;
using CodeMagic.UI.Blazor.Services;
using CodeMagic.UI;
using CodeMagic.UI.Presenters;
using CodeMagic.UI.Blazor.Models;
using CodeMagic.UI.Services;
using CodeMagic.Game.GameProcess;
using CodeMagic.Core.Common;
using CodeMagic.Game;
using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.Items.ItemsGeneration.Implementations;
using CodeMagic.Game.Items.Usable.Potions;
using CodeMagic.Game.MapGeneration.Dungeon;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Logging
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Services.AddLogging();

// Configuration
builder.Services.AddOptions<SettingsConfiguration>(SettingsConfiguration.ConfigSection);

// Services
builder.Services.AddSingleton<IWindowService, WindowService>();
builder.Services.AddSingleton<IApplicationController, ApplicationController>();
builder.Services.AddSingleton<IGameManager>(provider => new GameManager(
    provider.GetRequiredService<ISaveService>(), 
    provider.GetRequiredService<IOptions<SettingsConfiguration>>().Value.SavingInterval,
    provider.GetRequiredService<ILoggerFactory>(),
    provider.GetRequiredService<IDungeonMapGenerator>()));
builder.Services.AddSingleton<ISettingsService, SettingsService>();
builder.Services.AddSingleton<ISaveService, SaveService>();
builder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();
builder.Services.AddSingleton<IPerformanceMeter, PerformanceMeter>();
builder.Services.AddSingleton<IDungeonMapGenerator, DungeonMapGenerator>();
builder.Services.AddSingleton<IPotionDataService, PotionDataService>();
builder.Services.AddSingleton<IUsableItemsGenerator, UsableItemsGenerator>();
builder.Services.AddSingleton<IImagesStorageService, ImagesStorageService>();
builder.Services.AddSingleton<IDownloadFileService, DownloadFileService>();
builder.Services.AddSingleton<IFilesLoadService, FilesLoadService>();
builder.Services.AddSingleton<IItemsGenerator, ItemsGenerator>();
builder.Services.AddSingleton<IAncientSpellsService, AncientSpellsService>();
builder.Services.AddSingleton<ConfigurationProviderService>();

builder.Services.AddTransient(provider =>
    provider.GetRequiredService<ConfigurationProviderService>().GetItemGeneratorConfiguration());

// Windows
builder.Services.AddTransient<IMainMenuView, MainMenuModel>();
builder.Services.AddTransient<ISettingsView, SettingsModel>();
builder.Services.AddTransient<IWaitMessageView, WaitMessageModel>();
builder.Services.AddTransient<IGameView, GameViewModel>();

// Presenters
builder.Services.AddTransient<MainMenuPresenter>();
builder.Services.AddTransient<SettingsPresenter>();
builder.Services.AddTransient<PlayerInventoryPresenter>();
builder.Services.AddTransient<SpellBookPresenter>();
builder.Services.AddTransient<InGameMenuPresenter>();
builder.Services.AddTransient<PlayerDeathPresenter>();
builder.Services.AddTransient<WaitMessagePresenter>();
builder.Services.AddTransient<GameViewPresenter>();

// Starting
var application = builder.Build();

StaticLoggerFactory.Initialize(application.Services.GetRequiredService<ILoggerFactory>());

await application.Services.GetRequiredService<ConfigurationProviderService>().Initialize();

ItemsGeneratorManager.Initialize(application.Services.GetRequiredService<IItemsGenerator>());

await application.Services.GetRequiredService<AncientSpellsService>().Initialize();

await application.Services.GetRequiredService<IImagesStorageService>().Initialize();

application.Services.GetRequiredService<IApplicationController>().CreatePresenter<MainMenuPresenter>().Run();

await application.RunAsync();
