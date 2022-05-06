using CodeMagic.UI.Images;

namespace CodeMagic.Game
{
    public interface IImagesStorage
    {
        ISymbolsImage GetImage(string name);

        ISymbolsImage[] GetAnimation(string name);
    }
}