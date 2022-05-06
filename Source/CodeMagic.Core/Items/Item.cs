using System;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Items
{
    public abstract class Item : MapObjectBase, IItem
    {
        protected Item()
        {
            Id = Guid.NewGuid().ToString();
            Name = nameof(Item);
        }

        public string Id { get; set; }

        public virtual string Key { get; set; }

        public virtual ItemRareness Rareness { get; set; }

        public virtual int Weight { get; set; }

        public virtual bool Stackable => true;

        public bool Equals(IItem other)
        {
            if (other == null)
                return false;

            if (Stackable)
            {
                return string.Equals(Key, other.Key);
            }

            return string.Equals(Id, other.Id);
        }

        public override ZIndex ZIndex => ZIndex.AreaDecoration;

        public override ObjectSize Size => ObjectSize.Small;
    }
}