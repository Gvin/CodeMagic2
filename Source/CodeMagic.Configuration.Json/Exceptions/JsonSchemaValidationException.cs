namespace CodeMagic.Configuration.Json.Exceptions;

public class JsonSchemaValidationException : Exception
{
    public JsonSchemaValidationException(string[] errors, string type)
    : base($"Found {errors.Length} error when validating JSON schema for type {type}")
    {
        Errors = errors;
    }

    public string[] Errors { get; }
}