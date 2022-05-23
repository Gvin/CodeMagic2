using CodeMagic.Game.Configuration.Levels;

namespace CodeMagic.Configuration.Json.Levels;

public class LevelsConfiguration : ILevelsConfiguration
{
    public IPlayerLevelsConfiguration PlayerLevels => new PlayerLevelsConfiguration();
}

public class PlayerLevelsConfiguration : IPlayerLevelsConfiguration
{
    public int XpMultiplier => 20;
    public int XpLevelPower => 1;
}
