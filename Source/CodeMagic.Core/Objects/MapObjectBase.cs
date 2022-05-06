namespace CodeMagic.Core.Objects
{
    public abstract class MapObjectBase : IMapObject
    {
        protected MapObjectBase()
        {
            Name = nameof(MapObjectBase);
        }

        public virtual string Name { get; set; }

        public virtual bool BlocksMovement => false;

        public virtual bool BlocksProjectiles => false;

        public virtual bool IsVisible => true;

        public virtual bool BlocksVisibility => false;

        public virtual bool BlocksAttack => false;

        public virtual bool BlocksEnvironment => false;

        public abstract ZIndex ZIndex { get; }

        public abstract ObjectSize Size { get; }

        public virtual bool Equals(IMapObject other)
        {
            return ReferenceEquals(other, this);
        }
    }
}