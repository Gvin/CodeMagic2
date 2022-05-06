using CodeMagic.UI.Images;

namespace CodeMagic.Game
{
    public interface IWorldImageProvider
    {
        ISymbolsImage GetWorldImage(IImagesStorage storage);
    }
}