using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.DecorativeObjects;

[Serializable]
public class FireObject : MapObjectBase, IDynamicObject, ILightObject, IWorldImageProvider
{
    private const LightLevel SmallFireLightLevel = LightLevel.Dusk2;
    private const LightLevel MediumFireLightLevel = LightLevel.Dim2;
    private const LightLevel BigFireLightLevel = LightLevel.Medium;

    private const int MediumFireTemperature = 1000;
    private const int BigFireTemperature = 1500;
    public const int SmallFireTemperature = 600;

    private const string ImageSmall = "Fire_Small";
    private const string ImageMedium = "Fire_Medium";
    private const string ImageBig = "Fire_Big";

    private readonly SymbolsAnimationsManager _animations;

    public FireObject()
    {
        _animations = new SymbolsAnimationsManager(
            TimeSpan.FromMilliseconds(500),
            AnimationFrameStrategy.Random);
    }

    public override string Name => "Fire";

    public int Temperature { get; set; }

    public override ObjectSize Size => ObjectSize.Huge;

    public UpdateOrder UpdateOrder => UpdateOrder.Early;

    public bool Updated { get; set; }

    public override ZIndex ZIndex => ZIndex.AreaDecoration;

    private LightLevel LightPower
    {
        get
        {
            if (Temperature >= BigFireTemperature)
                return BigFireLightLevel;
            if (Temperature >= MediumFireTemperature)
                return MediumFireLightLevel;
            return SmallFireLightLevel;
        }
    }

    public ILightSource[] LightSources => new ILightSource[]
    {
        new StaticLightSource(LightPower)
    };

    public ISymbolsImage GetWorldImage(IImagesStorage storage)
    {
        var animationName = GetAnimationName();
        return _animations.GetImage(storage, animationName);
    }

    private string GetAnimationName()
    {
        if (Temperature >= BigFireTemperature)
            return ImageBig;
        if (Temperature >= MediumFireTemperature)
            return ImageMedium;
        return ImageSmall;
    }

    public void Update(Point position)
    {
        var cell = CurrentGame.Map.GetCell(position);
        if (cell.Temperature() < SmallFireTemperature)
        {
            CurrentGame.Map.RemoveObject(position, this);
            return;
        }

        Temperature = cell.Temperature();
    }
}