using System.Runtime.Serialization;

namespace CodeMagic.UI.Blazor.Exceptions;

[Serializable]
public class ApplicationLoadException : Exception
{
    public ApplicationLoadException()
    {
    }

    protected ApplicationLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ApplicationLoadException(string? message) : base(message)
    {
    }

    public ApplicationLoadException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}