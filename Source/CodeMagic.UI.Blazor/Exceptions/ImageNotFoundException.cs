using System.Runtime.Serialization;

namespace CodeMagic.UI.Blazor.Exceptions;

[Serializable]
public class ImageNotFoundException : Exception
{
    public ImageNotFoundException()
    : base("Image not found")
    {
    }

    public ImageNotFoundException(string imageName)
    : base($"Image {imageName} not found")
    {
    }

    protected ImageNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}