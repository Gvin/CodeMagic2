using CodeMagic.Core.Area;
using CodeMagic.Core.Objects;
using CodeMagic.Game;
using CodeMagic.Game.Images;
using CodeMagic.Game.Objects.ObjectEffects;
using CodeMagic.UI.Services;

namespace CodeMagic.UI.Blazor.Drawing;

public interface ICellImageService
{
    ISymbolsImage GetCellImage(IAreaMapCell cell);
}

public class CellImageService : ICellImageService
{
    private const int MapCellImageSize = 3;
    private static readonly TimeSpan DamageMarksLifeTime = TimeSpan.FromSeconds(2);
    private static readonly ISymbolsImage EmptyImage = new SymbolsImage(MapCellImageSize, MapCellImageSize);

    private readonly IImagesStorageService _imagesStorage;
    private readonly WorldImagesFactory _worldImagesFactory;
    private readonly LightLevelManager _lightLevelManager;

    public CellImageService(IImagesStorageService imagesStorage, ISettingsService settingsService)
    {
        _imagesStorage = imagesStorage;
        _worldImagesFactory = new WorldImagesFactory(imagesStorage);
        _lightLevelManager = new LightLevelManager(settingsService.Brightness);
    }

    public ISymbolsImage GetCellImage(IAreaMapCell? cell)
    {
        if (cell == null)
            return EmptyImage;

        var objectsImages = cell.Objects
            .Where(obj => obj.IsVisible)
            .OrderBy(obj => obj.ZIndex)
            .Select(GetObjectImage)
            .Where(img => img != null)
            .ToArray();

        var image = objectsImages.FirstOrDefault();
        if (image == null)
            return EmptyImage;

        foreach (var objectImage in objectsImages.Skip(1))
        {
            if (objectImage != null)
            {
                image = CombineImages(image, objectImage);
            }
        }

        image = _lightLevelManager.ApplyLightLevel(image, cell.LightLevel);
        return ApplyObjectEffects(cell, image);
    }

    private static ISymbolsImage CombineImages(ISymbolsImage bottom, ISymbolsImage top)
    {
        return SymbolsImage.Combine(bottom, top);
    }

    private ISymbolsImage ApplyObjectEffects(IAreaMapCell cell, ISymbolsImage image)
    {
        var bigObject = cell.Objects.OfType<IDestroyableObject>().FirstOrDefault(obj => obj.BlocksMovement);
        if (bigObject == null || !bigObject.ObjectEffects.Any())
            return image;

        var latestEffect = bigObject.ObjectEffects
            .OfType<ObjectEffect>()
            .Where(rec => rec.CreatedAt + DamageMarksLifeTime > DateTime.Now)
            .MaxBy(obj => obj.CreatedAt);

        if (latestEffect == null)
            return image;

        var effectImage = latestEffect.GetEffectImage(image.Width, image.Height, _imagesStorage);

        return SymbolsImage.Combine(image, effectImage);
    }

    private ISymbolsImage? GetObjectImage(IMapObject mapObject)
    {
        return _worldImagesFactory.GetImage(mapObject);
    }
}