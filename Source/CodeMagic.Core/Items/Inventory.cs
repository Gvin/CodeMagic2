using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Exceptions;

namespace CodeMagic.Core.Items
{
    public interface IInventory
	{
        event EventHandler<ItemEventArgs> ItemAdded;

        event EventHandler<ItemEventArgs> ItemRemoved;

        IInventoryStack[] Stacks { get; }

        int ItemsCount { get; }

        void AddItem(IItem item);

        void AddItems(IEnumerable<IItem> items);

        void RemoveItem(IItem item);

        void Update();

        int GetWeight();

        IItem GetItem(string itemKey);

        /// <summary>
        /// Gets specific item from the inventory by it's id.
        /// Returns null if the item is not found.
        /// </summary>
        /// <param name="itemId">Id of the item.</param>
        /// <param name="errorIfNotFound">Throw error if item with such id is not in the inventory.</param>
        IItem GetItemById(string itemId, bool errorIfNotFound = false);

        /// <summary>
        /// Gets specific item of specific type from the inventory by it's id.
        /// Returns null if the item is not found.
        /// </summary>
        /// <typeparam name="T">Expected item type.</typeparam>
        /// <param name="itemId">Id of the item.</param>
        /// <param name="errorIfNotFound">Throw error if item with such id is not in the inventory.</param>
        T GetItemById<T>(string itemId, bool errorIfNotFound = false) where T : IItem;

        bool Contains(string itemKey);
    }

    public class Inventory : IInventory
    {
        public event EventHandler<ItemEventArgs> ItemAdded;
        public event EventHandler<ItemEventArgs> ItemRemoved; 

        public Inventory()
        {
            Stacks = new List<IInventoryStack>();
        }

        public Inventory(IEnumerable<IItem> items)
        {
            Stacks = new List<IInventoryStack>();
            AddItems(items);
        }

        public List<IInventoryStack> Stacks { get; set; }

        public void AddItems(IEnumerable<IItem> items)
        {
            foreach (var item in items)
            {
                AddItem(item);
            }
        }

        public void AddItem(IItem item)
        {
            lock (Stacks)
            {
                if (item is IDecayItem decayItem)
                {
                    decayItem.Decayed += Item_Decayed;
                }

                if (!item.Stackable)
                {
                    Stacks.Add(new InventoryStack(item));
                    return;
                }

                var existingStack = GetItemStack(item);
                if (existingStack != null)
                {
                    existingStack.Add(item);
                }
                else
                {
                    Stacks.Add(new InventoryStack(item));
                }

                ItemAdded?.Invoke(this, new ItemEventArgs(item));
            }
        }

        private void Item_Decayed(object sender, EventArgs args)
        {
            var item = (IItem) sender;
            RemoveItem(item);
        }

        public void Update()
        {
            foreach (var stack in Stacks.ToArray())
            {
                foreach (var item in stack.Items.OfType<IDecayItem>().ToArray())
                {
                    item.Update();
                }
            }
        }

        public void RemoveItem(IItem item)
        {
            lock (Stacks)
            {
                var existingStack = GetItemStack(item);
                if (existingStack == null)
                {
                    throw new InvalidOperationException($"Item {item.Key} not found in inventory.");
                }

                if (item is IDecayItem decayItem)
                {
                    decayItem.Decayed -= Item_Decayed;
                }

                existingStack.Remove(item);
                if (existingStack.Count == 0)
                {
                    Stacks.Remove(existingStack);
                }

                ItemRemoved?.Invoke(this, new ItemEventArgs(item));
            }
        }

        public int ItemsCount => Stacks.Sum(stack => stack.Count);

        IInventoryStack[] IInventory.Stacks => Stacks.ToArray();

		public int GetWeight()
        {
            return Stacks.Sum(stack => stack.Weight);
        }

        public IItem GetItem(string itemKey)
        {
            return Stacks.FirstOrDefault(stack => string.Equals(stack.TopItem.Key, itemKey))?.TopItem;
        }

        public IItem GetItemById(string itemId, bool errorIfNotFound = false)
        {
            var result = GetItemByIdOrNull(itemId);
            if (result == null && errorIfNotFound)
            {
                throw ItemNotFoundException.ById(itemId); 
            }

            return result;
        }

        private IItem GetItemByIdOrNull(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                return null;
            }

            return Stacks.FirstOrDefault(stack => stack.Items.Any(item => string.Equals(item.Id, itemId)))?.Items
                .FirstOrDefault(item => string.Equals(item.Id, itemId));
        }

        public T GetItemById<T>(string itemId, bool errorIfNotFound = false) where T : IItem
        {
            var item = GetItemById(itemId, errorIfNotFound);

            if (item == null)
            {
                return default;
            }

            return (T)item;
        }

        public bool Contains(string itemKey)
        {
            return Stacks.Any(stack => string.Equals(stack.TopItem.Key, itemKey));
        }

        private IInventoryStack GetItemStack(IItem item)
        {
            return Stacks.FirstOrDefault(stack => stack.CheckItemMatches(item));
        }
    }

    public class ItemEventArgs : EventArgs
    {
        public ItemEventArgs(IItem item)
        {
            Item = item;
        }

        public IItem Item { get; }
    }

    public interface IInventoryStack
    {
        IItem[] Items { get; }

        IItem TopItem { get; }

        int Weight { get; }

        int Count { get; }

        void Add(IItem item);

        void Remove(IItem item);

        bool CheckItemMatches(IItem item);
    }


    public class InventoryStack : IInventoryStack
    {
        public InventoryStack()
        {
        }

        public InventoryStack(IItem item)
        {
            Items = new List<IItem> {item};
        }

        public List<IItem> Items { get; set; }

        IItem[] IInventoryStack.Items => Items.ToArray();

        public IItem TopItem => Items.Last();

        public int Weight => Items.Sum(item => item.Weight);

        public int Count => Items.Count;


        public void Add(IItem item)
        {
            if (!item.Stackable)
                throw new ArgumentException("Stackable item expected.");
            if (!CheckItemMatches(item))
                throw new ArgumentException("Item doesn't match stack.");

            Items.Add(item);
        }

        public void Remove(IItem item)
        {
            if (!Items.Contains(item))
                throw new ArgumentException("Item not found in stack.");

            Items.Remove(item);
        }

        public bool CheckItemMatches(IItem item)
        {
            return item.Equals(Items.First());
        }
    }
}