﻿using System;
using System.Drawing;
using CodeMagic.Core.Area;
using CodeMagic.Game.Drawing;
using CodeMagic.UI.Services;

namespace CodeMagic.UI.Drawing;

public interface ILightLevelManager
{
    ISymbolsImage ApplyLightLevel(ISymbolsImage image, LightLevel lightData);
}

public class LightLevelManager : ILightLevelManager
{
    private readonly ISettingsService _settingsService;

    public LightLevelManager(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public ISymbolsImage ApplyLightLevel(ISymbolsImage image, LightLevel lightData)
    {
        if (lightData != LightLevel.Darkness)
        {
            var i = 0;
        }

        var result = new SymbolsImage(image.Width, image.Height);
        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                var originalCell = image[x, y];
                var cell = result[x, y];

                cell.Symbol = originalCell.Symbol;

                if (originalCell.Color.HasValue)
                {
                    cell.Color = ApplyLightLevel(originalCell.Color.Value, lightData);
                }

                if (originalCell.BackgroundColor.HasValue)
                {
                    cell.BackgroundColor = ApplyLightLevel(originalCell.BackgroundColor.Value, lightData);
                }
            }
        }

        return result;
    }

    private Color ApplyLightLevel(Color color, LightLevel lightLevel)
    {
        var lightLevelPercent = GetLightLevelPercent(lightLevel);
        var red = Math.Min((int)(color.R * lightLevelPercent), 255);
        var green = Math.Min((int)(color.G * lightLevelPercent), 255);
        var blue = Math.Min((int)(color.B * lightLevelPercent), 255);

        var darkenedColor = Color.FromArgb(red, green, blue);
        return darkenedColor;
    }

    private float GetLightLevelPercent(LightLevel light)
    {
        return (int)light * _settingsService.Brightness;
    }
}