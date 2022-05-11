using CodeMagic.Game.Images;

namespace CodeMagic.Game
{
    public interface IInventoryImageProvider
    {
        ISymbolsImage GetInventoryImage(IImagesStorageService storage);
    }
}