namespace CodeMagic.Game.Drawing;

public interface ISymbolsImage
{
    int Width { get; }

    int Height { get; }

    SymbolsImage.Pixel this[int x, int y] { get; }
}