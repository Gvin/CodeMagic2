using CodeMagic.Configuration.Json.Exceptions;
using CodeMagic.Configuration.Json.ItemGenerator;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace CodeMagic.Configuration.Json;

public interface IJsonConfigurationLoader
{
    IItemGeneratorConfiguration? LoadItemGeneratorConfiguration(Stream stream);
}

public class JsonConfigurationLoader : IJsonConfigurationLoader
{
    private readonly ILogger<JsonConfigurationLoader> _logger;

    public JsonConfigurationLoader(ILogger<JsonConfigurationLoader> logger)
    {
        _logger = logger;
    }

    public IItemGeneratorConfiguration? LoadItemGeneratorConfiguration(Stream stream)
    {
        return LoadConfigurationFile<ItemGeneratorConfiguration>(stream);
    }

    private  T? LoadConfigurationFile<T>(Stream fileStream)
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