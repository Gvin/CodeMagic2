using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Drawing;

namespace CodeMagic.Game.Objects.SolidObjects;

[Serializable]
public class EnergyWall : MapObjectBase, IDynamicObject, ILightObject, IWorldImageProvider
{
    private const string ImageHighEnergy = "EnergyWall_HighEnergy";
    private const string ImageMediumEnergy = "EnergyWall_MediumEnergy";
    private const string ImageLowEnergy = "EnergyWall_LowEnergy";

    private const int MediumEnergy = 3;
    private const int HighEnergy = 10;

    public EnergyWall(int lifeTime)
    {
        EnergyLeft = lifeTime;
    }

    public EnergyWall()
    {
    }

    public int EnergyLeft { get; set; }

    public override string Name => "Energy Wall";

    public UpdateOrder UpdateOrder => UpdateOrder.Early;

    public override ObjectSize Size => ObjectSize.Huge;

    public ILightSource[] LightSources => new ILightSource[]
    {
        new StaticLightSource(CodeSpell.DefaultLightLevel)
    };

    public bool Updated { get; set; }

    public override bool BlocksMovement => true;

    public override bool BlocksAttack => true;

    public override bool BlocksProjectiles => true;

    public override bool BlocksEnvironment => true;

    public override ZIndex ZIndex => ZIndex.Wall;

    public void Update(Point position)
    {
        EnergyLeft--;
        if (EnergyLeft > 0)
            return;

        CurrentGame.Map.RemoveObject(position, this);
    }

    public ISymbolsImage GetWorldImage(IImagesStorageService storage)
    {
        if (EnergyLeft >= HighEnergy)
            return storage.GetImage(ImageHighEnergy);
        if (EnergyLeft >= MediumEnergy)
            return storage.GetImage(ImageMediumEnergy);
        return storage.GetImage(ImageLowEnergy);
    }
}