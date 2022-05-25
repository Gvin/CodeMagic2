using CodeMagic.Game.Drawing;

namespace CodeMagic.Game.Objects.ObjectEffects;

public class SpellCastEffect : ObjectEffect
{
    private const string ImageName = "Effect_SpellCast";

    public override ISymbolsImage GetEffectImage(int width, int height, IImagesStorageService imagesStorage)
    {
        return imagesStorage.GetImage(ImageName);
    }
}