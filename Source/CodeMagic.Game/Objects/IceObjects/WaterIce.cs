using System;
using CodeMagic.Game.Drawing;
using CodeMagic.Game.Objects.LiquidObjects;

namespace CodeMagic.Game.Objects.IceObjects;

[Serializable]
public class WaterIce : AbstractIce, IWorldImageProvider
{
    private const string ImageSmall = "Ice_Water_Small";
    private const string ImageMedium = "Ice_Water_Medium";
    private const string ImageBig = "Ice_Water_Big";
    private const string ObjectType = "WaterIce";

    public const int WaterIceMinVolumeForEffect = 50;

    public WaterIce(int volume)
    {
        Volume = volume;
    }

    public WaterIce()
    {
    }

    public override string LiquidType => WaterLiquid.LiquidType;

    public override string Name => "Ice";

    protected override int MinVolumeForEffect => WaterIceMinVolumeForEffect;

    public override string Type => ObjectType;

    protected override ILiquid CreateLiquid(int volume)
    {
        return new WaterLiquid(volume);
    }

    public ISymbolsImage GetWorldImage(IImagesStorageService storage)
    {
        if (Volume >= Configuration.MaxVolumeBeforeSpread)
            return storage.GetImage(ImageBig);

        var halfSpread = Configuration.MaxVolumeBeforeSpread / 2;
        if (Volume >= halfSpread)
            return storage.GetImage(ImageMedium);

        return storage.GetImage(ImageSmall);
    }
}