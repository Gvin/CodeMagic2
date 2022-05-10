using System;
using CodeMagic.Core.Objects;

namespace CodeMagic.Game.Objects.SolidObjects;

[Serializable]
public class CaveWall : SolidWallBase
{
    public override string Name => "Cave Wall";

    protected override string ImageNormal => "Wall_Cave";

    protected override string ImageBottom => "Wall_Cave_Bottom";

    protected override string ImageRight => "Wall_Cave_Right";

    protected override string ImageBottomRight => "Wall_Cave_Bottom_Right";

    protected override string ImageCorner => "Wall_Cave_Corner";

    public override bool CanConnectTo(IMapObject mapObject)
    {
        return mapObject is CaveWall;
    }
}