using CodeMagic.Configuration.Json.ItemGenerator;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema.Generation;

var outputPath = args[0];
outputPath = outputPath.Replace("\\", "/");

Console.WriteLine($"Generating configuration schema. Output path: {outputPath}");

var schemaGenerator = new JSchemaGenerator();
schemaGenerator.GenerationProviders.Add(new StringEnumGenerationProvider());
var schema = schemaGenerator.Generate(typeof(ItemGeneratorConfiguration));

Console.WriteLine("Writing schema to file...");

using var streamWriter = File.CreateText($"{outputPath}/ItemGeneratorConfiguration.Schema.json");
schema.WriteTo(new JsonTextWriter(streamWriter));

Console.WriteLine("Done.");