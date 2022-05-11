using System.Threading.Tasks;
using CodeMagic.Game.Images;

namespace CodeMagic.Game;

public interface IImagesStorageService
{
    ISymbolsImage GetImage(string name);

    ISymbolsImage[] GetAnimation(string name);

    Task Initialize();
}