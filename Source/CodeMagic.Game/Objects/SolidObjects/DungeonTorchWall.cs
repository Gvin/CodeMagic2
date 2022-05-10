using System;
using CodeMagic.Core.Objects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.SolidObjects;

[Serializable]
public class DungeonTorchWall : TorchWallBase, IWorldImageProvider
{
    private const string ImageNormal = "Wall_Dungeon";
    private const string ImageBottom = "Wall_Dungeon_Bottom_Torch";
    private const string ImageRight = "Wall_Dungeon_Right_Torch";
    private const string ImageBottomRight = "Wall_Dungeon_Bottom_Right_Torch";
    private const string ImageCorner = "Wall_Dungeon_Corner";

    public static DungeonTorchWall Create()
    {
        return new DungeonTorchWall();
    }

    private readonly ISymbolsAnimationsManager _animationsManager;

    public DungeonTorchWall()
    {
        _animationsManager = new SymbolsAnimationsManager(TimeSpan.FromMilliseconds(300),
            AnimationFrameStrategy.OneByOneStartFromRandom);
    }

    public override string Name => "Dungeon Wall";

    public override bool CanConnectTo(IMapObject mapObject)
    {
        return mapObject is DungeonWall or DungeonTorchWall or DungeonDoor;
    }

    public ISymbolsImage GetWorldImage(IImagesStorage storage)
    {
        if (!HasConnectedTile(0, 1) && !HasConnectedTile(1, 0))
        {
            return _animationsManager.GetImage(storage, ImageBottomRight);
        }

        if (!HasConnectedTile(0, 1))
        {
            return _animationsManager.GetImage(storage, ImageBottom);
        }

        if (!HasConnectedTile(1, 0))
        {
            return _animationsManager.GetImage(storage, ImageRight);
        }

        if (!HasConnectedTile(1, 1))
        {
            return storage.GetImage(ImageCorner);
        }

        return storage.GetImage(ImageNormal);
    }
}