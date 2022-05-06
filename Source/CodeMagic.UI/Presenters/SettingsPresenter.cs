using System;
using System.Linq;
using CodeMagic.UI.Services;

namespace CodeMagic.UI.Presenters
{
    public interface ISettingsView : IView
    {
        event EventHandler IncreaseFontSize;

        event EventHandler DecreaseFontSize;

        event EventHandler IncreaseBrightness;

        event EventHandler DecreaseBrightness;

        event EventHandler IncreaseSavingInterval;

        event EventHandler DecreaseSavingInterval;

        event EventHandler Exit;

        string FontSizeName { get; set; }

        float Brightness { get; set; }

        int SavingInterval { get; set; }
    }

    public class SettingsPresenter : IPresenter
    {
        private const float BrightnessStep = 0.1f;
        private const float BrightnessMin = 1.0f;
        private const float BrightnessMax = 2.0f;

        private const int SavingIntervalMax = 10;
        private const int SavingIntervalMin = 5;

        private readonly ISettingsView view;
        private readonly ISettingsService settings;

        public SettingsPresenter(ISettingsView view, ISettingsService settings)
        {
            this.view = view;
            this.settings = settings;

            this.view.Brightness = settings.Brightness;
            this.view.SavingInterval = settings.SavingInterval;
            this.view.FontSizeName = GetFontSizeName(settings.FontSize);

            this.view.Exit += View_Exit;

            this.view.IncreaseFontSize += View_IncreaseFontSize;
            this.view.DecreaseFontSize += View_DecreaseFontSize;

            this.view.IncreaseBrightness += View_IncreaseBrightness;
            this.view.DecreaseBrightness += View_DecreaseBrightness;

            this.view.IncreaseSavingInterval += View_IncreaseSavingInterval;
            this.view.DecreaseSavingInterval += View_DecreaseSavingInterval;
        }

        private void View_IncreaseBrightness(object sender, EventArgs e)
        {
            view.Brightness = Math.Min(BrightnessMax, Math.Max(BrightnessMin, view.Brightness + BrightnessStep));
            settings.Brightness = view.Brightness;
            settings.Save();
        }

        private void View_DecreaseBrightness(object sender, EventArgs e)
        {
            view.Brightness = Math.Min(BrightnessMax, Math.Max(BrightnessMin, view.Brightness - BrightnessStep));
            settings.Brightness = view.Brightness;
            settings.Save();
        }

        private void View_IncreaseSavingInterval(object sender, EventArgs e)
        {
            view.SavingInterval = Math.Min(SavingIntervalMax, Math.Max(SavingIntervalMin, view.SavingInterval + 1));
            settings.SavingInterval = view.SavingInterval;
            settings.Save();
        }

        private void View_DecreaseSavingInterval(object sender, EventArgs e)
        {
            view.SavingInterval = Math.Min(SavingIntervalMax, Math.Max(SavingIntervalMin, view.SavingInterval - 1));
            settings.SavingInterval = view.SavingInterval;
            settings.Save();
        }

        private void View_DecreaseFontSize(object sender, EventArgs e)
        {
            SwitchFontSize(false);
            view.FontSizeName = GetFontSizeName(settings.FontSize);
        }

        private void View_IncreaseFontSize(object sender, EventArgs e)
        {
            SwitchFontSize(true);
            view.FontSizeName = GetFontSizeName(settings.FontSize);
        }

        private void View_Exit(object sender, EventArgs e)
        {
            view.Close();
        }

        public void Run()
        {
            view.Show();
        }

        private string GetFontSizeName(FontSizeMultiplier fontSize)
        {
            switch (fontSize)
            {
                case FontSizeMultiplier.X1:
                    return "x1";
                case FontSizeMultiplier.X2:
                    return "x2";
                default:
                    throw new ArgumentException($"Unknown font size: {fontSize}");
            }
        }

        private void SwitchFontSize(bool forward)
        {
            var diff = forward ? 1 : -1;
            var size = settings.FontSize;
            var sizes = Enum.GetValues(typeof(FontSizeMultiplier)).Cast<FontSizeMultiplier>().ToList();

            var currentIndex = sizes.IndexOf(size);
            var nextIndex = currentIndex + diff;
            nextIndex = Math.Max(0, nextIndex);
            nextIndex = Math.Min(sizes.Count - 1, nextIndex);

            settings.FontSize = sizes[nextIndex];
            settings.Save();
        }
    }
}