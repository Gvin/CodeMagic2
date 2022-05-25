using CodeMagic.Core.Objects;
using CodeMagic.Core.Statuses;
using CodeMagic.Game;
using CodeMagic.Game.Drawing;
using CodeMagic.Game.Statuses;

namespace CodeMagic.UI.Drawing;

public interface IWorldImagesFactory
{
    ISymbolsImage GetImage(object objectToDraw);
}

public class WorldImagesFactory : IWorldImagesFactory
{
    private const string ImageStatusOnFire = "Status_OnFire";
    private const string ImageStatusOily = "Status_Oily";
    private const string ImageStatusWet = "Status_Wet";
    private const string ImageStatusBlind = "Status_Blind";
    private const string ImageStatusParalyzed = "Status_Paralyzed";
    private const string ImageStatusFrozen = "Status_Frozen";
    private const string ImageStatusOverweight = "Status_Overweight";
    private const string ImageStatusManaDisturbed = "Status_ManaDisturbed";

    private readonly IImagesStorageService _imagesStorage;

    public WorldImagesFactory(IImagesStorageService imagesStorage)
    {
        _imagesStorage = imagesStorage;
    }

    public ISymbolsImage GetImage(object objectToDraw)
    {
        var objectImage = GetObjectImage(objectToDraw);
        if (objectImage == null)
            return null;

        if (objectToDraw is IDestroyableObject destroyable)
        {
            objectImage = ApplyDestroyableStatuses(destroyable, objectImage);
        }

        return objectImage;
    }

    private ISymbolsImage ApplyDestroyableStatuses(IDestroyableObject destroyable, ISymbolsImage image)
    {
        if (destroyable.Statuses.Contains(OnFireObjectStatus.StatusType))
        {
            return ApplyStatusImage(image, ImageStatusOnFire);
        }

        if (destroyable.Statuses.Contains(ParalyzedObjectStatus.StatusType))
        {
            return ApplyStatusImage(image, ImageStatusParalyzed);
        }

        if (destroyable.Statuses.Contains(FrozenObjectStatus.StatusType))
        {
            return ApplyStatusImage(image, ImageStatusFrozen);
        }

        if (destroyable.Statuses.Contains(BlindObjectStatus.StatusType))
        {
            return ApplyStatusImage(image, ImageStatusBlind);
        }

        if (destroyable.Statuses.Contains(ManaDisturbedObjectStatus.StatusType))
        {
            return ApplyStatusImage(image, ImageStatusManaDisturbed);
        }

        if (destroyable.Statuses.Contains(OverweightObjectStatus.StatusType))
        {
            return ApplyStatusImage(image, ImageStatusOverweight);
        }

        if (destroyable.Statuses.Contains(OilyObjectStatus.StatusType))
        {
            return ApplyStatusImage(image, ImageStatusOily);
        }

        if (destroyable.Statuses.Contains(WetObjectStatus.StatusType))
        {
            return ApplyStatusImage(image, ImageStatusWet);
        }

        return image;
    }

    private ISymbolsImage ApplyStatusImage(ISymbolsImage initialImage, string statusImageName)
    {
        var statusImage = _imagesStorage.GetImage(statusImageName);
        return SymbolsImage.Combine(initialImage, statusImage);
    }

    private ISymbolsImage GetObjectImage(object objectToDraw)
    {
        if (objectToDraw is IWorldImageProvider selfProvider)
        {
            return selfProvider.GetWorldImage(_imagesStorage);
        }

        return null;
    }
}