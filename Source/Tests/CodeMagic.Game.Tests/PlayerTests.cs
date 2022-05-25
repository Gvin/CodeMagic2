using CodeMagic.Game.Items.Custom;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Tests.Common;
using NUnit.Framework;

namespace CodeMagic.Game.Tests;

[TestFixture]
public class PlayerTests
{
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    public void LightSources_ReturnsLightSourcesFromEquipped(int count)
    {
        // ARRANGE
        var player = CreatePlayer();

        if (count > 0)
        {
            var torch = new TorchItem();
            player.Inventory.AddItem(torch);
            player.Equipment.EquipHoldable(torch, true);
        }

        if (count > 1)
        {
            var torch = new TorchItem();
            player.Inventory.AddItem(torch);
            player.Equipment.EquipHoldable(torch, false);
        }

        // ACT
        var sources = player.LightSources;

        // ASSERT
        Assert.AreEqual(count, sources.Length);
    }

    private static Player CreatePlayer()
    {
        StaticLoggerFactory.Initialize(TestLoggerHelper.CreateLoggerFactory());

        var player = new Player();
        player.Initialize();

        return player;
    }
}