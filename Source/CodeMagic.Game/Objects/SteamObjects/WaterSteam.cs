using System;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Images;
using CodeMagic.Game.Objects.LiquidObjects;

namespace CodeMagic.Game.Objects.SteamObjects;

[Serializable]
public class WaterSteam : AbstractSteam, IWorldImageProvider
{
    private const string ImageSmall = "Water_Steam_Small";
    private const string ImageMedium = "Water_Steam_Medium";
    private const string ImageBig = "Water_Steam_Big";
    private const string SteamType = "WaterSteam";

    private const int ThicknessBig = 70;
    private const int ThicknessMedium = 30;

    private readonly ISymbolsAnimationsManager _animations;

    public WaterSteam(int volume) 
        : this()
    {
        Volume = volume;
    }

    public WaterSteam()
    {
        _animations = new SymbolsAnimationsManager(
            TimeSpan.FromSeconds(1),
            AnimationFrameStrategy.Random);
    }

    protected override string LiquidType => WaterLiquid.LiquidType;

    public override string Name => "Water Steam";

    public override string Type => SteamType;

    public override ISpreadingObject Separate(int volume)
    {
        Volume -= volume;
        return new WaterSteam(volume);
    }

    protected override ILiquid CreateLiquid(int volume)
    {
        return new WaterLiquid(volume);
    }

    public ISymbolsImage GetWorldImage(IImagesStorageService storage)
    {
        var animationName = GetAnimationName();
        return _animations.GetImage(storage, animationName);
    }

    private string GetAnimationName()
    {
        if (Thickness >= ThicknessBig)
            return ImageBig;
        if (Thickness >= ThicknessMedium)
            return ImageMedium;
        return ImageSmall;
    }
}