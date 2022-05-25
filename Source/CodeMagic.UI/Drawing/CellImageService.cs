using System;
using System.Drawing;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Objects;
using CodeMagic.Game;
using CodeMagic.Game.Drawing;
using CodeMagic.Game.Objects.ObjectEffects;
using CodeMagic.UI.Services;
using Microsoft.Extensions.Logging;

namespace CodeMagic.UI.Drawing;

public interface ICellImageService
{
    ISymbolsImage GetCellImage(IAreaMapCell cell);
}

public class CellImageService : ICellImageService
{
    private const int MapCellImageSize = 7;
    private static readonly TimeSpan DamageMarksLifeTime = TimeSpan.FromSeconds(2);
    private static readonly ISymbolsImage EmptyImage = new SymbolsImage(MapCellImageSize, MapCellImageSize);

    private readonly IImagesStorageService _imagesStorage;
    private readonly IWorldImagesFactory _worldImagesFactory;
    private readonly ILightLevelManager _lightLevelManager;
    private readonly ISettingsService _settingsService;
    private readonly ILogger<CellImageService> _logger;

    public CellImageService(
        IImagesStorageService imagesStorage,
        IWorldImagesFactory worldImagesFactory,
        ILightLevelManager lightLevelManager,
        ISettingsService settingsService,
        ILogger<CellImageService> logger)
    {
        _imagesStorage = imagesStorage;
        _worldImagesFactory = worldImagesFactory;
        _lightLevelManager = lightLevelManager;
        _settingsService = settingsService;
        _logger = logger;
    }

    public ISymbolsImage GetCellImage(IAreaMapCell cell)
    {
        try
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
                    image = SymbolsImage.Combine(image, objectImage);
                }
            }

            image = _lightLevelManager.ApplyLightLevel(image, cell.LightLevel);
            image = ApplyObjectEffects(cell, image);
            return ApplyDebugData(cell, image);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting image for the cell.");
            throw;
        }
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

    private ISymbolsImage ApplyDebugData(IAreaMapCell cell, ISymbolsImage image)
    {
        var debugDataImage = new SymbolsImage(image.Width, image.Height);

        if (_settingsService.DebugDrawLightLevel)
        {
            var lightLevelString = ((int)cell.LightLevel).ToString();

            if (lightLevelString.Length == 2)
            {
                debugDataImage.SetPixel(image.Width - 2, 0, lightLevelString[0], Color.Black, Color.Yellow);
            }

            debugDataImage.SetPixel(image.Width - 1, 0, lightLevelString.Last(), Color.Black, Color.Yellow);
        }

        return SymbolsImage.Combine(image, debugDataImage);
    }

    private ISymbolsImage GetObjectImage(IMapObject mapObject)
    {
        return _worldImagesFactory.GetImage(mapObject);
    }
}