using CodeMagic.Configuration.Json.Exceptions;
using CodeMagic.Configuration.Json.ItemGenerator;
using CodeMagic.Configuration.Json.Liquids;
using CodeMagic.Configuration.Json.Monsters;
using CodeMagic.Configuration.Json.Physics;
using CodeMagic.Configuration.Json.Spells;
using CodeMagic.Game.Configuration.Liquids;
using CodeMagic.Game.Configuration.Monsters;
using CodeMagic.Game.Configuration.Physics;
using CodeMagic.Game.Configuration.Spells;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace CodeMagic.Configuration.Json;

public class JsonConfigurationLoader : IConfigurationLoader
{
    public IItemGeneratorConfiguration? LoadItemGeneratorConfiguration(Stream stream)
    {
        return LoadConfigurationFile<ItemGeneratorConfiguration>(stream);
    }

    public IPhysicsConfiguration? LoadPhysicsConfiguration(Stream stream)
    {
        return LoadConfigurationFile<PhysicsConfiguration>(stream);
    }

    public ILiquidsConfiguration? LoadLiquidsConfiguration(Stream stream)
    {
        return LoadConfigurationFile<LiquidsConfiguration>(stream);
    }

    public ISpellsConfiguration? LoadSpellsConfiguration(Stream stream)
    {
        return LoadConfigurationFile<SpellsConfiguration>(stream);
    }

    public IMonstersConfiguration? LoadMonstersConfiguration(Stream stream)
    {
        return LoadConfigurationFile<MonstersConfiguration>(stream);
    }

    private static T? LoadConfigurationFile<T>(Stream fileStream)
    {
        var reader = new JsonTextReader(new StreamReader(fileStream));

        var schemaGenerator = new JSchemaGenerator();
        schemaGenerator.GenerationProviders.Add(new StringEnumGenerationProvider());

        var validatingReader = new JSchemaValidatingReader(reader)
        {
            Schema = schemaGenerator.Generate(typeof(T))
        };

        IList<string> messages = new List<string>();
        validatingReader.ValidationEventHandler += (_, arg) => messages.Add(arg.Message);

        var result = new JsonSerializer().Deserialize<T>(validatingReader);

        if (messages.Count > 0)
        {
            throw new JsonSchemaValidationException(messages.ToArray(), nameof(T));
        }

        return result;
    }
}