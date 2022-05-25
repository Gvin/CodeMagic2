using System.Threading.Tasks;
using CodeMagic.Game.Drawing;

namespace CodeMagic.Game;

public interface IImagesStorageService
{
    ISymbolsImage GetImage(string name);

    ISymbolsImage[] GetAnimation(string name);

    Task Initialize();
}