namespace CodeMagic.UI.Services
{
    public interface ISettingsService
    {
        float Brightness { get; set; }

        bool DebugDrawTemperature { get; }

        bool DebugDrawLightLevel { get; }

        bool DebugDrawMagicEnergy { get; }

        FontSizeMultiplier FontSize { get; set; }

        int SavingInterval { get; set; }

        int MinActionsInterval { get; }

        bool DebugWriteMapToFile { get; }

        void Save();
    }

    public enum FontSizeMultiplier
    {
        X1,
        X2
    }
}