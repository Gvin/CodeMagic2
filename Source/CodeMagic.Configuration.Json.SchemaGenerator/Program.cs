using CodeMagic.Configuration.Json.ItemGenerator;
using CodeMagic.Configuration.Json.Liquids;
using CodeMagic.Configuration.Json.Monsters;
using CodeMagic.Configuration.Json.Physics;
using CodeMagic.Configuration.Json.Spells;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema.Generation;

var outputPath = args[0];
outputPath = outputPath.Replace("\\", "/");

Console.WriteLine($"Generating configuration schema. Output path: {outputPath}");

CreateSchema<ItemGeneratorConfiguration>(outputPath, "ItemGeneratorConfiguration.Schema");
CreateSchema<PhysicsConfiguration>(outputPath, "PhysicsConfiguration.Schema");
CreateSchema<LiquidsConfiguration>(outputPath, "LiquidsConfiguration.Schema");
CreateSchema<SpellsConfiguration>(outputPath, "SpellsConfiguration.Schema");
CreateSchema<MonstersConfiguration>(outputPath, "MonstersConfiguration.Schema");

Console.WriteLine("Done.");

void CreateSchema<T>(string schemaOutputPath, string fileName)
{
    Console.WriteLine($"Generating schema for {typeof(T).Name}");

    var schemaGenerator = new JSchemaGenerator();
    schemaGenerator.GenerationProviders.Add(new StringEnumGenerationProvider());
    var schema = schemaGenerator.Generate(typeof(T));
    schema.Id = new Uri($"http://code-magic2.com/{typeof(T).Name}");

    Console.WriteLine("Writing schema to file...");

    using var streamWriter = File.CreateText($"{schemaOutputPath}/{fileName}.json");
    schema.WriteTo(new JsonTextWriter(streamWriter));
}
