using CodeMagic.Game.Configuration.Levels;
using CodeMagic.Game.Configuration.Liquids;
using CodeMagic.Game.Configuration.Monsters;
using CodeMagic.Game.Configuration.Physics;
using CodeMagic.Game.Configuration.Spells;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;

namespace CodeMagic.Configuration.Json;

public interface IConfigurationLoader
{
    IItemGeneratorConfiguration? LoadItemGeneratorConfiguration(Stream stream);

    IPhysicsConfiguration? LoadPhysicsConfiguration(Stream stream);

    ILiquidsConfiguration? LoadLiquidsConfiguration(Stream stream);

    ISpellsConfiguration? LoadSpellsConfiguration(Stream stream);

    IMonstersConfiguration? LoadMonstersConfiguration(Stream stream);
}