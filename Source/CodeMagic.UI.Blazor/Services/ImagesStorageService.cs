using System.Text.RegularExpressions;
using CodeMagic.Game;
using CodeMagic.Game.Drawing;
using CodeMagic.UI.Blazor.Exceptions;

namespace CodeMagic.UI.Blazor.Services;

public class ImagesStorageService : IImagesStorageService
{
    private const string ImagesCompiledFile = "images-compiled.json";
    private const string BatchesRegex = "(.*)(?:_\\d*)$";

    private readonly IFilesLoadService _filesLoadService;
    private readonly ILogger<ImagesStorageService> _logger;

    private readonly Dictionary<string, ISymbolsImage> _imagesCache;
    private readonly Dictionary<string, List<ISymbolsImage>> _animationsCache;

    public ImagesStorageService(
        IFilesLoadService filesLoadService,
        ILogger<ImagesStorageService> logger)
    {
        _filesLoadService = filesLoadService;
        _logger = logger;

        _imagesCache = new Dictionary<string, ISymbolsImage>();
        _animationsCache = new Dictionary<string, List<ISymbolsImage>>();
    }

    public async Task Initialize()
    {
        _logger.LogDebug("Loading Images");

        var images = await _filesLoadService.LoadFileAsync<Dictionary<string, SymbolsImage>>(ImagesCompiledFile);
        if (images == null)
        {
            throw new Exception("Images list not found or empty");
        }

        var batchRegex = new Regex(BatchesRegex);

        _imagesCache.Clear();

        foreach (var (imageName, image) in images)
        {
            var match = batchRegex.Match(imageName);

            if (!match.Success) // Image is not part of animation
            {
                _imagesCache.Add(imageName, image);
                continue;
            }

            var batchName = match.Groups[1].Value.ToLower();
            if (!_animationsCache.ContainsKey(batchName))
            {
                _animationsCache.Add(batchName, new List<ISymbolsImage>());
            }
            _animationsCache[batchName].Add(image);
        }
    }

    public ISymbolsImage GetImage(string name)
    {
        if (!_imagesCache.ContainsKey(name))
        {
            throw new ImageNotFoundException(name);
        }

        return _imagesCache[name];
    }

    public ISymbolsImage[] GetAnimation(string name)
    {
        var key = name.ToLower();
        if (!_animationsCache.ContainsKey(key))
        {
            throw new AnimationNotFoundException(name);
        }

        return _animationsCache[key].ToArray();
    }
}