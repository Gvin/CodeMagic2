using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Drawing;

namespace CodeMagic.Game.Objects.Floor;

[Serializable]
public class FloorObject : MapObjectBase, IWorldImageProvider
{
    public static FloorObject Create(Type floorType)
    {
        return new FloorObject { FloorType = floorType };
    }

    public Type FloorType { get; set; }

    public override string Name => GetName(FloorType);

    private static string GetName(Type type)
    {
        switch (type)
        {
            case Type.Stone:
                return "Stone";
            default:
                throw new ArgumentException($"Unknown floor type: {type}");
        }
    }

    public override ZIndex ZIndex => ZIndex.Floor;

    public override ObjectSize Size => ObjectSize.Huge;

    public ISymbolsImage GetWorldImage(IImagesStorageService storage)
    {
        return storage.GetImage(GetWorldImageName(FloorType));
    }

    private static string GetWorldImageName(Type floorType)
    {
        switch (floorType)
        {
            case Type.Stone:
                return RandomHelper.GetRandomElement(
                    "Floor_Stone_V1",
                    "Floor_Stone_V2",
                    "Floor_Stone_V3");
            default:
                throw new ArgumentException($"Unknown floor type: {floorType}");
        }
    }

    public enum Type
    {
        Stone
    }
}