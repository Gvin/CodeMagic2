using CodeMagic.Game.Drawing;

namespace CodeMagic.Game
{
    public interface IInventoryImageProvider
    {
        ISymbolsImage GetInventoryImage(IImagesStorageService storage);
    }
}