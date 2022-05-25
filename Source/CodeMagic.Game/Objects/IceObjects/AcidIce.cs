using System;
using CodeMagic.Game.Drawing;
using CodeMagic.Game.Objects.LiquidObjects;

namespace CodeMagic.Game.Objects.IceObjects;

[Serializable]
public class AcidIce : AbstractIce, IWorldImageProvider
{
    private const string ImageSmall = "Ice_Acid_Small";
    private const string ImageMedium = "Ice_Acid_Medium";
    private const string ImageBig = "Ice_Acid_Big";
    private const string ObjectType = "AcidIce";

    public const int AcidIceMinVolumeForEffect = 50;

    public AcidIce(int volume)
    {
        Volume = volume;
    }

    public AcidIce()
    {
    }

    public override string LiquidType => AcidLiquid.LiquidType;

    public override string Name => "Acid Ice";

    protected override int MinVolumeForEffect => AcidIceMinVolumeForEffect;

    public override string Type => ObjectType;

    protected override ILiquid CreateLiquid(int volume)
    {
        return new AcidLiquid(volume);
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