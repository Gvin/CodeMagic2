using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Images;

namespace CodeMagic.Game.Objects.DecorativeObjects;

[Serializable]
public class DungeonTorchPost : MapObjectBase, ILightObject, IWorldImageProvider
{
    public DungeonTorchPost Create()
    {
        return new DungeonTorchPost();
    }

    private const string AnimationName = "Decoratives_TorchPost";

    private readonly ISymbolsAnimationsManager _animationsManager;

    public DungeonTorchPost()
    {
        _animationsManager = new SymbolsAnimationsManager(
            TimeSpan.FromMilliseconds(500),
            AnimationFrameStrategy.Random);
    }

    public override string Name => "Torch Post";

    public override ZIndex ZIndex => ZIndex.BigDecoration;

    public override ObjectSize Size => ObjectSize.Huge;

    public ILightSource[] LightSources => new ILightSource[]
    {
        new StaticLightSource(LightLevel.Medium),
    };

    public ISymbolsImage GetWorldImage(IImagesStorageService storage)
    {
        return _animationsManager.GetImage(storage, AnimationName);
    }
}