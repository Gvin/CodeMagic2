using System;
using System.Collections.Generic;
using CodeMagic.Game.Drawing;

namespace CodeMagic.Game.Objects;

public interface ISymbolsAnimationsManager
{
    ISymbolsImage GetImage(IImagesStorageService storage, string animationName);
}

public class SymbolsAnimationsManager : ISymbolsAnimationsManager
{
    private readonly Dictionary<string, ISymbolsAnimation> _managers;
    private readonly TimeSpan _changeInterval;
    private readonly AnimationFrameStrategy _frameStrategy;

    public SymbolsAnimationsManager(
        TimeSpan changeInterval,
        AnimationFrameStrategy frameStrategy = AnimationFrameStrategy.OneByOneStartFromZero)
    {
        _changeInterval = changeInterval;
        _frameStrategy = frameStrategy;
        _managers = new Dictionary<string, ISymbolsAnimation>();
    }

    public ISymbolsImage GetImage(IImagesStorageService storage, string animationName)
    {
        if (!_managers.ContainsKey(animationName))
        {
            _managers.Add(animationName,
                new SymbolsAnimation(storage.GetAnimation(animationName), _changeInterval, _frameStrategy));
        }

        return _managers[animationName].GetCurrentFrame();
    }
}