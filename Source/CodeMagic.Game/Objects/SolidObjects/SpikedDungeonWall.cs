using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Game.JournalMessages;

namespace CodeMagic.Game.Objects.SolidObjects
{
    [Serializable]
    public class SpikedDungeonWall : SolidWallBase, ICollideDamageObject
    {
        private const int MinDamage = 10;
        private const int MaxDamage = 30;

        public override string Name => "Spiked Dungeon Wall";

        protected override string ImageNormal => "Wall_Spiked";

        protected override string ImageBottom => "Wall_Spiked_Bottom";

        protected override string ImageRight => "Wall_Spiked_Right";

        protected override string ImageBottomRight => "Wall_Spiked_Bottom_Right";

        protected override string ImageCorner => "Wall_Spiked_Corner";

        public override bool CanConnectTo(IMapObject mapObject)
        {
            return mapObject is SpikedDungeonWall or DungeonWall or DungeonDoor or DungeonTorchWall;
        }

        public void Damage(IDestroyableObject collidedObject, Point position)
        {
            var damage = RandomHelper.GetRandomValue(MinDamage, MaxDamage);
            collidedObject.Damage(position, damage, Element.Piercing);
            CurrentGame.Journal.Write(new EnvironmentDamageMessage(collidedObject, damage, Element.Piercing), collidedObject);
        }
    }
}