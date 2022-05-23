using CodeMagic.Configuration.Json.Common;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Configuration.Treasure;

namespace CodeMagic.Configuration.Json.Treasure;

internal class TreasureConfiguration : ITreasureConfiguration
{
    public ITreasureLevelsConfiguration[]? Levels { get; init; }
}

internal class TreasureLevelsConfiguration : ITreasureLevelsConfiguration
{
    public int StartLevel { get; init; }

    public int EndLevel { get; init; }

    public Dictionary<string, ILootConfiguration>? Loot { get; set; }
}
