using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Objects.SolidObjects
{
    public abstract class DoorBase : WallBase, IUsableObject
    {
        protected DoorBase()
        {
            Closed = true;
        }

        public bool Closed { get; set; }

        public override bool BlocksMovement => Closed;

        public override bool BlocksAttack => Closed;

        public override bool BlocksProjectiles => Closed;

        public override bool BlocksVisibility => Closed;

        public override bool BlocksEnvironment => Closed;

        public bool CanUse => Closed;

        public void Use(IGameCore game, Point position)
        {
            if (!Closed)
                return;

            Closed = false;
        }
    }
}