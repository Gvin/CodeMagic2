using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.Objects.Creatures.Loot;

namespace CodeMagic.Game.Objects.Furniture;

[Serializable]
public class ContainerObject : FurnitureObject, IUsableObject, IDynamicObject
{
    public ContainerObject(int lootLevelIncrement, int level, string containerType)
    {
        var lootLevel = level + lootLevelIncrement;
        var loot = new TreasureLootGenerator(lootLevel, containerType).GenerateLoot();

        Inventory = new Inventory(loot);
    }

    public ContainerObject()
    {
    }

    public IInventory Inventory { get; set; }

    public bool CanUse => true;

    public void Use(GameCore<Player> game, Point position)
    {
        game.Journal.Write(new ContainerOpenMessage(Name));
        DialogsManager.Provider.OpenInventoryDialog(Name, Inventory);
    }

    public override void OnDeath(Point position)
    {
        base.OnDeath(position);

        foreach (var stack in Inventory.Stacks)
        {
            foreach (var item in stack.Items)
            {
                CurrentGame.Map.AddObject(position, item);
            }
        }
    }

    public void Update(Point position)
    {
        Inventory.Update();
    }

    public bool Updated { get; set; }

    public UpdateOrder UpdateOrder => UpdateOrder.Late;
}
