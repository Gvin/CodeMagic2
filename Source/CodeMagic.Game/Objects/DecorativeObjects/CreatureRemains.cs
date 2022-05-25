using System;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Drawing;

namespace CodeMagic.Game.Objects.DecorativeObjects;

[Serializable]
public class CreatureRemains : MapObjectBase, IWorldImageProvider
{
    private const string ImageBloodSmall = "Remains_Blood_Small";
    private const string ImageBloodMedium = "Remains_Blood_Medium";
    private const string ImageBloodBig = "Remains_Blood_Big";

    private const string ImageGreenBloodSmall = "Remains_GreenBlood_Small";
    private const string ImageGreenBloodMedium = "Remains_GreenBlood_Medium";
    private const string ImageGreenBloodBig = "Remains_GreenBlood_Big";

    private const string ImageBonesWhiteSmall = "Remains_Bones_Small";
    private const string ImageBonesWhiteMedium = "Remains_Bones_Medium";

    public static CreatureRemains Create(RemainsType remainsType)
    {
        return new CreatureRemains
        {
            RemainsType = remainsType
        };
    }

    public override string Name => GetName(RemainsType);

    public RemainsType RemainsType { get; set; }

    public override ZIndex ZIndex => ZIndex.GroundDecoration;

    public override ObjectSize Size => ObjectSize.Huge;

    private static string GetName(RemainsType type)
    {
        switch (type)
        {
            case RemainsType.BloodRedSmall:
            case RemainsType.BloodRedMedium:
            case RemainsType.BloodRedBig:
                return "Blood";
            case RemainsType.BloodGreenSmall:
            case RemainsType.BloodGreenMedium:
            case RemainsType.BloodGreenBig:
                return "Green Blood";
            case RemainsType.BonesWhiteSmall:
            case RemainsType.BonesWhiteMedium:
                return "Bones";
            default:
                throw new ArgumentException($"Unknown remains type: {type}");
        }
    }

    public ISymbolsImage GetWorldImage(IImagesStorageService storage)
    {
        var imageName = GetWorldImageName();
        return storage.GetImage(imageName);
    }

    private string GetWorldImageName()
    {
        switch (RemainsType)
        {
            case RemainsType.BloodRedSmall:
                return ImageBloodSmall;
            case RemainsType.BloodRedMedium:
                return ImageBloodMedium;
            case RemainsType.BloodRedBig:
                return ImageBloodBig;

            case RemainsType.BloodGreenSmall:
                return ImageGreenBloodSmall;
            case RemainsType.BloodGreenMedium:
                return ImageGreenBloodMedium;
            case RemainsType.BloodGreenBig:
                return ImageGreenBloodBig;

            case RemainsType.BonesWhiteSmall:
                return ImageBonesWhiteSmall;
            case RemainsType.BonesWhiteMedium:
                return ImageBonesWhiteMedium;

            default:
                throw new ArgumentException($"Unknown remains type: {RemainsType}");
        }
    }
}

public enum RemainsType
{
    BloodRedSmall,
    BloodRedMedium,
    BloodRedBig,

    BloodGreenSmall,
    BloodGreenMedium,
    BloodGreenBig,

    BonesWhiteSmall,
    BonesWhiteMedium
}