using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items
{
    public interface IEquippedImageProvider
    {
        ISymbolsImage GetEquippedImage(Player player, IImagesStorage imagesStorage);

        int EquippedImageOrder { get; }
    }
}