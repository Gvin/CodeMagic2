using System;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Drawing;

namespace CodeMagic.Game.Objects.SolidObjects;

[Serializable]
public class DungeonDoor : DoorBase, IWorldImageProvider
{
    private const string ImageOpenedHorizontal = "Door_Dungeon_Opened_Horizontal";
    private const string ImageOpenedVertical = "Door_Dungeon_Opened_Vertical";
    private const string ImageClosedVertical = "Door_Dungeon_Closed_Vertical";
    private const string ImageClosedHorizontal = "Door_Dungeon_Closed_Horizontal";

    public override string Name => "Door";

    public override bool CanConnectTo(IMapObject mapObject)
    {
        return mapObject is DungeonWall or DungeonTorchWall or DungeonDoor;
    }

    public ISymbolsImage GetWorldImage(IImagesStorageService storage)
    {
        if (Closed)
        {
            if (HasConnectedTile(0, -1) && HasConnectedTile(0, 1))
                return storage.GetImage(ImageClosedVertical);

            return storage.GetImage(ImageClosedHorizontal);
        }

        if (HasConnectedTile(0, -1) && HasConnectedTile(0, 1))
            return storage.GetImage(ImageOpenedVertical);

        return storage.GetImage(ImageOpenedHorizontal);
    }
}