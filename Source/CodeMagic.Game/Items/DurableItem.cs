using System;
using CodeMagic.Core.Items;

namespace CodeMagic.Game.Items
{
    public interface IDurableItem : IDecayItem
    {
        int Durability { get; set; }

        int MaxDurability { get; }
    }

    [Serializable]
    public abstract class DurableItem : EquipableItem, IDurableItem
    {
        public event EventHandler Decayed;

        public int MaxDurability { get; set; }

        public sealed override bool Stackable => false;

        public int Durability { get; set; }

        int IDurableItem.Durability
        {
            get => Durability;
            set
            {
                Durability = Math.Min(MaxDurability, Math.Max(0, value));

                if (Durability == 0)
                {
                    Decayed?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void Update()
        {
            // Do nothing.
        }
    }
}