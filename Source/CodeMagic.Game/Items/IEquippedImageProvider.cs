using CodeMagic.Core.Objects;
using CodeMagic.Game.Drawing;

namespace CodeMagic.Game.Items
{
    public interface IEquippedImageProvider
    {
        ISymbolsImage GetEquippedImage(IPlayer player, IImagesStorageService imagesStorage);

        int EquippedImageOrder { get; }
    }
}