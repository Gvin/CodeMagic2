using CodeMagic.Game;
using CodeMagic.Game.Images;
using CodeMagic.UI.Blazor.Exceptions;

namespace CodeMagic.UI.Blazor.Services;

public class ImagesStorageService : IImagesStorageService
{
    private const string ImagesListFile = "images-list.json";
    private const string ImagesFolder = "images";

    private readonly IFilesLoadService _filesLoadService;
    private readonly ILogger<ImagesStorageService> _logger;

    private readonly Dictionary<string, ISymbolsImage> _imagesCache;
    private readonly Dictionary<string, ISymbolsImage[]> _animationsCache;

    public ImagesStorageService(IFilesLoadService filesLoadService, ILogger<ImagesStorageService> logger)
    {
        _filesLoadService = filesLoadService;
        _logger = logger;

        _imagesCache = new Dictionary<string, ISymbolsImage>();
        _animationsCache = new Dictionary<string, ISymbolsImage[]>();
    }

    public async Task Initialize()
    {
        var imagesList = await _filesLoadService.LoadFileAsync<ImagesList>(ImagesListFile);
        if (imagesList == null)
        {
            throw new Exception("Images list not found or empty");
        }

        _logger.LogDebug("Loading Images");

        foreach (var imagePath in imagesList.Images ?? Array.Empty<string>())
        {
            var imageName = GetImageName(imagePath);

            var path = $"{ImagesFolder}/{imagePath}";

            _logger.LogDebug("Loading image \"{ImageName}\" from path \"{ImagePath}\"", imageName, path);

            var image = await _filesLoadService.LoadFileAsync<SymbolsImage>(path);

            _imagesCache[imageName] = image ?? throw new FileNotFoundException("Image file not found.", path);
        }

        _logger.LogDebug("Loading Animations");

        foreach (var animation in imagesList.Animations ?? Array.Empty<Animation>())
        {
            if (string.IsNullOrWhiteSpace(animation.Name) || animation.Frames == null || animation.Frames.Length == 0)
            {
                throw new Exception($"Invalid config for animation \"{animation.Name}\"");
            }

            _logger.LogDebug(
                "Loading animation \"{AnimationName}\" containing {FramesCount} frames",
                animation.Name,
                animation.Frames.Length);

            var frames = await LoadAnimationFrames(animation.Frames);
            _animationsCache[animation.Name] = frames;
        }
    }

    private async Task<ISymbolsImage[]> LoadAnimationFrames(string[] frames)
    {
        var result = new List<ISymbolsImage>();

        foreach (var framePath in frames)
        {
            var path = $"{ImagesFolder}/{framePath}";
            var image = await _filesLoadService.LoadFileAsync<SymbolsImage>(path);

            if (image == null)
            {
                throw new FileNotFoundException("Animation frame not found.", path);
            }

            result.Add(image);
        }

        return result.ToArray();
    }

    private string GetImageName(string imagePath)
    {
        var noExtension = imagePath.Replace(".simg", "");
        var lastSlashIndex = noExtension.LastIndexOf('/');
        return noExtension.Substring(lastSlashIndex + 1);
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
        if (!_animationsCache.ContainsKey(name))
        {
            throw new AnimationNotFoundException(name);
        }

        return _animationsCache[name];
    }

    [Serializable]
    public class ImagesList
    {
        public string[]? Images { get; set; }

        public Animation[]? Animations { get; set; }
    }

    [Serializable]
    public class Animation
    {
        public string? Name { get; set; }

        public string[]? Frames { get; set; }
    }
}