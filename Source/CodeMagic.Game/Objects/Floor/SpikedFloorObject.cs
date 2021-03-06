using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Game.Images;
using CodeMagic.Game.JournalMessages;

namespace CodeMagic.Game.Objects.Floor;

[Serializable]
public class SpikedFloorObject : MapObjectBase, IStepReactionObject, IWorldImageProvider, IDangerousObject
{
    private const string WorldImageName = "Trap_SpikedFloor";
    private const int MinDamage = 2;
    private const int MaxDamage = 5;

    public static SpikedFloorObject Create()
    {
        return new SpikedFloorObject();
    }

    public override string Name => "Spikes";

    public ISymbolsImage GetWorldImage(IImagesStorageService storage)
    {
        return storage.GetImage(WorldImageName);
    }

    public override ZIndex ZIndex => ZIndex.FloorCover;

    public override ObjectSize Size => ObjectSize.Huge;

    public Point ProcessStepOn(Point position, ICreatureObject target, Point initialTargetPosition)
    {
        var damage = RandomHelper.GetRandomValue(MinDamage, MaxDamage);
        target.Damage(position, damage, Element.Piercing);
        CurrentGame.Journal.Write(new DealDamageMessage(this, target, damage, Element.Piercing), target);
        return position;
    }
}