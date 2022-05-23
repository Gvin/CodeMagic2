using CodeMagic.Configuration.Json.Common;
using CodeMagic.Configuration.Json.Levels;
using CodeMagic.Configuration.Json.Treasure;
using CodeMagic.Core.Items;
using CodeMagic.Game.Configuration.Levels;
using CodeMagic.Game.Configuration.Treasure;
using CodeMagic.Game.Items;

namespace CodeMagic.Configuration.Json;

public class StaticConfigurationLoader
{
    public ILevelsConfiguration GetLevelsConfiguration() => new LevelsConfiguration();

    public ITreasureConfiguration GeTreasureConfiguration()
    {
        return new TreasureConfiguration
        {
            Levels = new ITreasureLevelsConfiguration[]
            {
                new TreasureLevelsConfiguration
                {
                    StartLevel = 1,
                    EndLevel = 200, // TODO: Add real configuration
                    Loot = new()
                    {
                        {
                            "shelf",
                            new LootConfiguration
                            {
                                Food = new SimpleLootConfiguration
                                {
                                    Count = ChanceConfiguration<int>.Create((70, 1), (30, 0))
                                },
                                SpellBook = new StandardLootConfiguration
                                {
                                    Count = ChanceConfiguration<int>.Create((10, 1), (90, 0)),
                                    Rareness = ChanceConfiguration<ItemRareness>.Create((100, ItemRareness.Trash))
                                },
                                Resource = new StandardLootConfiguration
                                {
                                    Count = ChanceConfiguration<int>.Create((20, 1), (80, 0)),
                                    Rareness = ChanceConfiguration<ItemRareness>.Create((100, ItemRareness.Trash))
                                }
                            }
                        },
                        {
                            "crate",
                            new LootConfiguration
                            {
                                Food = new SimpleLootConfiguration
                                {
                                    Count = ChanceConfiguration<int>.Create((20, 1), (80, 0)),
                                },
                                Weapon = new StandardLootConfiguration
                                {
                                    Count = ChanceConfiguration<int>.Create((50, 1), (50, 0)),
                                    Rareness = ChanceConfiguration<ItemRareness>.Create(
                                        (30, ItemRareness.Common),
                                        (70, ItemRareness.Trash))
                                },
                                Shield = new StandardLootConfiguration
                                {
                                    Count = ChanceConfiguration<int>.Create((30, 1), (70, 0)),
                                    Rareness = ChanceConfiguration<ItemRareness>.Create(
                                        (30, ItemRareness.Common),
                                        (70, ItemRareness.Trash))
                                },
                                Armor = new ArmorLootConfiguration
                                {
                                    Count = ChanceConfiguration<int>.Create((20, 1), (80, 0)),
                                    Rareness = ChanceConfiguration<ItemRareness>.Create(
                                        (30, ItemRareness.Common),
                                        (70, ItemRareness.Trash)),
                                    Class = ChanceConfiguration<ArmorClass>.Create(
                                        (90, ArmorClass.Leather),
                                        (10, ArmorClass.Mail))
                                }
                            }
                        },
                        {
                            "chest",
                            new LootConfiguration
                            {
                                Weapon = new StandardLootConfiguration
                                {
                                    Count = ChanceConfiguration<int>.Create((50, 1), (50, 0)),
                                    Rareness = ChanceConfiguration<ItemRareness>.Create(
                                        (70, ItemRareness.Trash),
                                        (30, ItemRareness.Common))
                                },
                                Shield = new StandardLootConfiguration
                                {
                                    Count = ChanceConfiguration<int>.Create((30, 1), (70, 0)),
                                    Rareness = ChanceConfiguration<ItemRareness>.Create(
                                        (70, ItemRareness.Trash),
                                        (30, ItemRareness.Common))
                                },
                                Armor = new ArmorLootConfiguration
                                {
                                    Count = ChanceConfiguration<int>.Create((20, 1), (80, 0)),
                                    Rareness = ChanceConfiguration<ItemRareness>.Create(
                                        (70, ItemRareness.Trash),
                                        (30, ItemRareness.Common)),
                                    Class = ChanceConfiguration<ArmorClass>.Create(
                                        (90, ArmorClass.Leather),
                                        (10, ArmorClass.Mail))
                                },
                                SpellBook = new StandardLootConfiguration
                                {
                                    Count = ChanceConfiguration<int>.Create((10, 1), (90, 0)),
                                    Rareness = ChanceConfiguration<ItemRareness>.Create((100, ItemRareness.Trash))
                                },
                                Resource = new StandardLootConfiguration
                                {
                                    Count = ChanceConfiguration<int>.Create((20, 1), (80, 0)),
                                    Rareness = ChanceConfiguration<ItemRareness>.Create((100, ItemRareness.Trash))
                                }
                            }
                        }
                    }
                }
            }
        };
    }
}