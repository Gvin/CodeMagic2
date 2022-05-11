using CodeMagic.Game.Images;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items
{
    public interface IEquippedImageProvider
    {
        ISymbolsImage GetEquippedImage(Player player, IImagesStorageService imagesStorage);

        int EquippedImageOrder { get; }
    }
}