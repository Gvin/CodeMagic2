using System;
using CodeMagic.Core.Objects;

namespace CodeMagic.Game.Objects.SolidObjects;

[Serializable]
public class DungeonWall : SolidWallBase
{
    public override string Name => "Dungeon Wall";

    protected override string ImageNormal => "Wall_Dungeon";

    protected override string ImageBottom => "Wall_Dungeon_Bottom";

    protected override string ImageRight => "Wall_Dungeon_Right";

    protected override string ImageBottomRight => "Wall_Dungeon_Bottom_Right";

    protected override string ImageCorner => "Wall_Dungeon_Corner";

    public override bool CanConnectTo(IMapObject mapObject)
    {
        return mapObject is DungeonWall || mapObject is DungeonTorchWall || mapObject is DungeonDoor;
    }
}