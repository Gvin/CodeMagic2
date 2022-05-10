namespace CodeMagic.Core.Objects
{
    public abstract class MapObjectBase : IMapObject
    {
        protected MapObjectBase()
        {
            Name = nameof(MapObjectBase);
            BlocksMovement = false;
            BlocksProjectiles = false;
            IsVisible = true;
            BlocksVisibility = false;
            BlocksAttack = false;
        }

        public virtual string Name { get; set; }

        public virtual bool BlocksMovement { get; set; }

        public virtual bool BlocksProjectiles { get; set; }

        public virtual bool IsVisible { get; set; }

        public virtual bool BlocksVisibility { get; set; }

        public virtual bool BlocksAttack { get; set; }

        public virtual bool BlocksEnvironment => false;

        public virtual ZIndex ZIndex { get; set; }

        public virtual ObjectSize Size { get; set; }

        public virtual bool Equals(IMapObject other)
        {
            return ReferenceEquals(other, this);
        }
    }
}