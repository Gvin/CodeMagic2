using CodeMagic.Game.Images;

namespace CodeMagic.Game
{
    public interface IWorldImageProvider
    {
        ISymbolsImage GetWorldImage(IImagesStorageService storage);
    }
}