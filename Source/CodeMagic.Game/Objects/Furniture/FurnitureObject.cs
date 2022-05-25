using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Drawing;
using CodeMagic.Game.Items.Materials;

namespace CodeMagic.Game.Objects.Furniture;

[Serializable]
public class FurnitureObject : DestroyableObject, IWorldImageProvider
{
    public string WorldImage { get; set; }

    public int MaxWoodCount { get; set; }

    public int MinWoodCount { get; set; }

    public override ZIndex ZIndex { get; set; }

    public override ObjectSize Size { get; set; }

    public override int MaxHealth { get; set; }

    public override bool BlocksMovement { get; set; }

    public ISymbolsImage GetWorldImage(IImagesStorageService storage)
    {
        return storage.GetImage(WorldImage);
    }

    public override void OnDeath(Point position)
    {
        base.OnDeath(position);

        var woodCount = RandomHelper.GetRandomValue(MinWoodCount, MaxWoodCount);
        for (var counter = 0; counter < woodCount; counter++)
        {
            CurrentGame.Map.AddObject(position, new Wood());
        }
    }
}
