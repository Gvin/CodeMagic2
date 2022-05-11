using System.Drawing;

namespace CodeMagic.UI.Blazor;

public static class ColorExtensions
{
    private const string DefaultColor = "white";

    public static string ToStyleString(this Color color)
    {
        return $"rgba({color.R},{color.G},{color.B},{color.A})";
    }

    public static string ToStyleString(this Color? color)
    {
        if (color == null)
        {
            return DefaultColor;
        }

        return color.Value.ToStyleString();
    }
}