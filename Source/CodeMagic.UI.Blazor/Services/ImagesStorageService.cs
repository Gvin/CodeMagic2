using System.Text.RegularExpressions;
using CodeMagic.Game;
using CodeMagic.Game.Drawing;
using CodeMagic.UI.Blazor.Exceptions;

namespace CodeMagic.UI.Blazor.Services;

public class ImagesStorageService : IImagesStorageService
{
    private const string ImagesListFile = "images-list.json";
    private const string ImagesFolder = "images";
    private const string BatchesRegex = "(.*)(?:_\\d*)$";

    private const string ImageInStorageKeyPrefix = "ImagesStorageService_Image_";

    private readonly IFilesLoadService _filesLoadService;
    private readonly ILocalStorageService _localStorageService;
    private readonly ILogger<ImagesStorageService> _logger;

    private readonly Dictionary<string, ISymbolsImage> _imagesCache;
    private readonly Dictionary<string, List<ISymbolsImage>> _animationsCache;

    public ImagesStorageService(
        IFilesLoadService filesLoadService,
        ILocalStorageService localStorageService,
        ILogger<ImagesStorageService> logger)
    {
        _filesLoadService = filesLoadService;
        _logger = logger;
        _localStorageService = localStorageService;

        _imagesCache = new Dictionary<string, ISymbolsImage>();
        _animationsCache = new Dictionary<string, List<ISymbolsImage>>();
    }

    private static string GetImageName(string imagePath)
    {
        var noExtension = imagePath.Replace(".simg", "");
        var lastSlashIndex = noExtension.LastIndexOf('/');
        return noExtension.Substring(lastSlashIndex + 1);
    }

    public async Task Initialize()
    {
        var imagesList = await _filesLoadService.LoadFileAsync<ImageRecord[]>(ImagesListFile);
        if (imagesList == null)
        {
            throw new Exception("Images list not found or empty");
        }

        _logger.LogDebug("Loading Images");

        var batchRegex = new Regex(BatchesRegex);

        var loadImageTasks = imagesList.Select(imageRecord => LoadImageRecord(imageRecord, batchRegex)).ToArray();

        await Task.WhenAll(loadImageTasks);
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

    private async Task LoadImageRecord(ImageRecord imageRecord, Regex batchRegex)
    {
        if (string.IsNullOrEmpty(imageRecord.FilePath))
        {
            throw new ApplicationLoadException("Empty file path for image record.");
        }

        var imageDataRecord = await LoadImage(imageRecord.FilePath, imageRecord.Hash);

        var match = batchRegex.Match(imageDataRecord.Name!);

        if (!match.Success)
        {
            _imagesCache[imageDataRecord.Name!] = imageDataRecord.Image!;
            return;
        }

        var batchName = match.Groups[1].Value.ToLower();
        if (!_animationsCache.ContainsKey(batchName))
        {
            _animationsCache.Add(batchName, new List<ISymbolsImage>());
        }
        _animationsCache[batchName].Add(imageDataRecord.Image!);
    }

    private async Task<ImageDataRecord> LoadImage(string filePath, int hash)
    {
        var imageName = GetImageName(filePath);

        var imageFromStorage = await LoadImageFromStorage(imageName);

        if (imageFromStorage != null && imageFromStorage.Hash == hash)
        {
            _logger.LogDebug("Image {ImageName} has no difference with cached. Taking from storage.", imageName);
            return imageFromStorage;
        }

        var path = $"{ImagesFolder}/{filePath}";

        _logger.LogDebug("Loading image \"{ImageName}\" from path \"{ImagePath}\"", imageName, path);

        var image = await _filesLoadService.LoadFileAsync<SymbolsImage>(path);

        _logger.LogDebug("Image {ImageName} loaded.", imageName);

        if (image == null)
        {
            throw new FileNotFoundException("Image file not found.", path);
        }

        var imageDataRecord = new ImageDataRecord
        {
            Name = imageName,
            Image = image,
            Hash = hash
        };

        _logger.LogDebug("Caching image {ImageName}", imageName);
        var key = GetImageStorageKey(imageName);
        await _localStorageService.SetAsync(key, imageDataRecord);

        return imageDataRecord;
    }

    private Task<ImageDataRecord?> LoadImageFromStorage(string name)
    {
        var key = GetImageStorageKey(name);
        return _localStorageService.GetAsync<ImageDataRecord>(key);
    }

    private static string GetImageStorageKey(string name)
    {
        return $"{ImageInStorageKeyPrefix}{name}";
    }

    [Serializable]
    public class ImageRecord
    {
        public string? FilePath { get; set; }

        public int Hash { get; set; }
    }

    [Serializable]
    public class ImageDataRecord
    {
        public string? Name { get; set; }

        public SymbolsImage? Image { get; set; }

        public int Hash { get; set; }
    }
}