using CodeMagic.Game.Drawing;

namespace CodeMagic.Game
{
    public interface IWorldImageProvider
    {
        ISymbolsImage GetWorldImage(IImagesStorageService storage);
    }
}