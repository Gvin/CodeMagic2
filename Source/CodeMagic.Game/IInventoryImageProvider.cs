using CodeMagic.UI.Images;

namespace CodeMagic.Game
{
    public interface IInventoryImageProvider
    {
        ISymbolsImage GetInventoryImage(IImagesStorage storage);
    }
}