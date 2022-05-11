using System;
using CodeMagic.Core.Objects.ObjectEffects;
using CodeMagic.Game.Images;

namespace CodeMagic.Game.Objects.ObjectEffects;

public abstract class ObjectEffect : IObjectEffect
{
    protected ObjectEffect()
    {
        CreatedAt = DateTime.Now;
    }

    public DateTime CreatedAt { get; }

    public abstract ISymbolsImage GetEffectImage(int width, int height, IImagesStorageService imagesStorage);
}