using System.Drawing;

namespace CodeMagic.Game.Images;

public interface ISymbolsImage
{
    int Width { get; }

    int Height { get; }

    void SetPixel(int x, int y, char? symbol, Color? color, Color? backgroundColor = null);

    void SetSymbolMap(char?[][] symbolMap);

    void SetColorMap(Color?[][] colorMap);

    void SetDefaultColor(Color color);

    void SetDefaultBackColor(Color? color);

    SymbolsImage.Pixel this[int x, int y] { get; }
}