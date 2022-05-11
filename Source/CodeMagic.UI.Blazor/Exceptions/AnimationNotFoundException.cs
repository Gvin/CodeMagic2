using System.Runtime.Serialization;

namespace CodeMagic.UI.Blazor.Exceptions;

[Serializable]
public class AnimationNotFoundException : Exception
{
    public AnimationNotFoundException()
    : base("Animation not found")
    {
    }

    public AnimationNotFoundException(string animationName)
    : base($"Animation {animationName} not found")
    {
    }

    protected AnimationNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}